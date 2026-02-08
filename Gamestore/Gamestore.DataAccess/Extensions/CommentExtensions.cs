using Gamestore.DataAccess.Entities;

namespace Gamestore.DataAccess.Extensions;

public static class CommentExtensions
{
    public static List<Comment> ToTree(this IEnumerable<Comment> flatComments)
    {
        var lookup = flatComments.ToDictionary(x => x.Id);
        var rootComments = new List<Comment>();

        foreach (var comment in flatComments)
        {
            if (comment.ParentCommentId.HasValue && lookup.TryGetValue(comment.ParentCommentId.Value, out var parent))
            {
                parent.ChildComments.Add(comment);
            }
            else
            {
                rootComments.Add(comment);
            }
        }

        return rootComments;
    }
}
