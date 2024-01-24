using Orleans.Concurrency;
using Orleans.EventSourcing;

namespace Runner.Grains.EventSourcing;

public record BlogCreatedEvent(string Content);
public record BlogCommentAddedEvent(string Comment);
public record BlogLikedEvent;
public record BlogDislikedEvent;


[Serializable]
[GenerateSerializer]
public class JournaledBlogPostGrainState
{
    [Id(0)]
    public bool Created { get; private set; } = false;
    [Id(1)]
    public string Content { get; private set; } = "";
    [Id(2)]
    public List<string> Comments { get; private set; } = [];
    [Id(3)]
    public long Likes { get; private set; } = 0;
    [Id(4)]
    public long Dislikes { get; private set; } = 0;

    public void Apply(BlogCreatedEvent blogCreatedEvent)
    {
        Created = true;
        Content = blogCreatedEvent.Content;
    }

    public void Apply(BlogCommentAddedEvent blogCommentAddedEvent)
    {
        Comments.Add(blogCommentAddedEvent.Comment);
    }

    public void Apply(BlogLikedEvent blogLikedEvent)
    {
        Likes++;
    }

    public void Apply(BlogDislikedEvent blogDislikedEvent)
    {
        Dislikes++;
    }
}

public class JournaledBlogPostGrain : JournaledGrain<JournaledBlogPostGrainState>, IBlogPostGrain
{
    public async ValueTask Create(string content)
    {
        if (State.Created)
        {
            throw new InvalidOperationException("Cannot create blog post -- Post already exists");
        }
        
        if (string.IsNullOrWhiteSpace(content))
        {
            throw new ArgumentException("Cannot create blog post -- Content cannot be empty", nameof(content));
        }

        RaiseEvent(new BlogCreatedEvent(content));
        await ConfirmEvents();
    }

    public async ValueTask AddComment(string comment)
    {
        ThrowIfNotCreated("Cannot add post comment");
     
        if (string.IsNullOrWhiteSpace(comment))
        {
            throw new ArgumentException("Cannot add post comment -- Comment cannot be empty", nameof(comment));
        }
        
        RaiseEvent(new BlogCommentAddedEvent(comment));
        await ConfirmEvents();
    }

    public async ValueTask Like()
    {
        ThrowIfNotCreated("Cannot like post");

        RaiseEvent(new BlogLikedEvent());
        await ConfirmEvents();
    }

    public async ValueTask Dislike()
    {
        ThrowIfNotCreated("Cannot dislike post");
        
        RaiseEvent(new BlogDislikedEvent());
        await ConfirmEvents();
    }

    public ValueTask<string> GetContent()
    {
        ThrowIfNotCreated("Cannot get post content");
        return ValueTask.FromResult(State.Content);
    }

    public ValueTask<IReadOnlyList<string>> GetAllComments()
    {
        ThrowIfNotCreated("Cannot get post comments");
        var comments = (IReadOnlyList<string>)State.Comments.AsReadOnly();
        return ValueTask.FromResult(comments);
    }

    public ValueTask<string> GetComment(int index)
    {
        ThrowIfNotCreated("Cannot get post comment");

        var comments = State.Comments;
        
        if (index >= comments.Count)
        {
            throw new IndexOutOfRangeException();
        }

        return ValueTask.FromResult(comments[index]);
    }

    public ValueTask<(long Likes, long Dislikes)> GetRating()
    {
        ThrowIfNotCreated("Cannot get post rating");

        return ValueTask.FromResult((State.Likes, State.Dislikes));
    }
    
    public ValueTask Deactivate()
    {
        DeactivateOnIdle();
        return ValueTask.CompletedTask;
    }

    private void ThrowIfNotCreated(string prefix)
    {
        if (!State.Created)
        {
            throw new InvalidOperationException(prefix + " -- Blog post does not exist");
        }
    }
}

[Reentrant]
public class ReentrantJournaledBlogPostGrain : JournaledBlogPostGrain;