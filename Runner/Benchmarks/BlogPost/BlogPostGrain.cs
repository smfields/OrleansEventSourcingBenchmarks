using Orleans.Runtime;

namespace Runner.Benchmarks.BlogPost;

public interface IBlogPostGrain : IGrainWithGuidKey
{
    public ValueTask Create(string content);
    public ValueTask AddComment(string comment);
    public ValueTask Like();
    public ValueTask Dislike();
    public ValueTask<string> GetContent();
    public ValueTask<IReadOnlyList<string>> GetAllComments();
    public ValueTask<string> GetComment(int index);
    public ValueTask<(long Likes, long Dislikes)> GetRating();
    public ValueTask Deactivate();
}

[Serializable]
[GenerateSerializer]
public class BlogPostGrainState
{
    [Id(0)]
    public string Content { get; set; } = "";
    [Id(1)]
    public List<string> Comments { get; set; } = [];
    [Id(2)]
    public long Likes { get; set; } = 0;
    [Id(3)]
    public long Dislikes { get; set; } = 0;
}

public class BlogPostGrain(IPersistentState<BlogPostGrainState> state) : Grain, IBlogPostGrain
{
    public async ValueTask Create(string content)
    {
        if (state.RecordExists)
        {
            throw new InvalidOperationException("Cannot create blog post -- Post already exists");
        }
        
        if (string.IsNullOrWhiteSpace(content))
        {
            throw new ArgumentException("Cannot create blog post -- Content cannot be empty", nameof(content));
        }

        state.State = new BlogPostGrainState
        {
            Content = content
        };
        await state.WriteStateAsync();
    }

    public async ValueTask AddComment(string comment)
    {
        ThrowIfNotCreated("Cannot add post comment");
     
        if (string.IsNullOrWhiteSpace(comment))
        {
            throw new ArgumentException("Cannot add post comment -- Comment cannot be empty", nameof(comment));
        }
        
        state.State.Comments.Add(comment);
        await state.WriteStateAsync();
    }

    public async ValueTask Like()
    {
        ThrowIfNotCreated("Cannot like post");

        state.State.Likes++;
        await state.WriteStateAsync();
    }

    public async ValueTask Dislike()
    {
        ThrowIfNotCreated("Cannot dislike post");
        
        state.State.Dislikes++;
        await state.WriteStateAsync();
    }

    public ValueTask<string> GetContent()
    {
        ThrowIfNotCreated("Cannot get post content");
        return ValueTask.FromResult(state.State.Content);
    }

    public ValueTask<IReadOnlyList<string>> GetAllComments()
    {
        ThrowIfNotCreated("Cannot get post comments");
        var comments = (IReadOnlyList<string>)state.State.Comments.AsReadOnly();
        return ValueTask.FromResult(comments);
    }

    public ValueTask<string> GetComment(int index)
    {
        ThrowIfNotCreated("Cannot get post comment");

        var comments = state.State.Comments;
        
        if (index >= comments.Count)
        {
            throw new IndexOutOfRangeException();
        }

        return ValueTask.FromResult(comments[index]);
    }

    public ValueTask<(long Likes, long Dislikes)> GetRating()
    {
        ThrowIfNotCreated("Cannot get post rating");

        return ValueTask.FromResult((state.State.Likes, state.State.Dislikes));
    }
    
    public ValueTask Deactivate()
    {
        DeactivateOnIdle();
        return ValueTask.CompletedTask;
    }

    private void ThrowIfNotCreated(string prefix)
    {
        if (!state.RecordExists)
        {
            throw new InvalidOperationException(prefix + " -- Blog post does not exist");
        }
    }
}