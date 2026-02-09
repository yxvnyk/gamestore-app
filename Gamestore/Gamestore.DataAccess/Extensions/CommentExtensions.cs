using Gamestore.DataAccess.Entities;
using Gamestore.Domain.Enums;

namespace Gamestore.DataAccess.Extensions;

public static class CommentExtensions
{
    private const string DeletedBodyText = "A comment/quote was deleted";

    public static List<Comment> ToTree(this IEnumerable<Comment> flatComments)
    {
        var lookup = flatComments.ToDictionary(x => x.Id);

        foreach (var c in flatComments)
        {
            c.ChildComments.Clear();
        }

        var rootComments = new List<Comment>();

        foreach (var comment in flatComments)
        {
            Comment? parent = (comment.ParentCommentId.HasValue && lookup.TryGetValue(comment.ParentCommentId.Value, out var p))
                ? p
                : null;

            comment.Body = GetFormattedBody(comment, parent);

            if (parent != null)
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

    private static string GetFormattedBody(Comment comment, Comment? parent)
    {
        if (comment.IsDeleted)
        {
            return DeletedBodyText;
        }

        if (parent is null)
        {
            return comment.Body;
        }

        // For replies and quotes, we want to show the parent comment's content (or a placeholder if it's deleted) in a specific format.
        return comment.Type switch
        {
            CommentType.Reply => FormatReply(comment.Body, parent.Name),
            CommentType.Quote => FormatQuote(comment.Body, parent),
            CommentType.Standard => comment.Body,
            _ => comment.Body,
        };
    }

    private static string FormatReply(string text, string authorName)
    {
        return $"[{authorName}], {text}";
    }

    private static string FormatQuote(string text, Comment parent)
    {
        var quotedText = parent.IsDeleted ? DeletedBodyText : parent.Body;
        return $"\"{quotedText}\" {text}";
    }
}