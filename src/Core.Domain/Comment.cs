using System;

namespace Arashi.Core.Domain
{
	/// <summary>
	/// Represents a comment.
	/// </summary>
	public class Comment
	{
		/// <summary>
		/// Identifier.
		/// </summary>
		public virtual int CommentId { get; set; }

		/// <summary>
		/// The name of the commenter.
		/// </summary>
		public virtual string Name { get; set; }

      /// <summary>
      /// Gets the name of the person who commented (either from the associated user or the explicit name).
      /// </summary>
      public virtual string AuthorName
      {
         get
         {
            return this.CreatedBy != null ? this.CreatedBy.FullName : this.Name;
         }
      }

		public virtual string Email { get; set; }

		/// <summary>
		/// The web site address of the commenter.
		/// </summary>
		public virtual string Url { get; set; }

		/// <summary>
		/// The comment.
		/// </summary>
		public virtual string CommentText { get; set; }

		/// <summary>
		/// IP address of the commenter.
		/// </summary>
		public virtual string UserIp { get; set; }
		public virtual string UserAgent { get; set; }

      public virtual CommentStatus Status { get; set; }

      public virtual CommentType Type { get; set; }

		/// <summary>
		/// The <see cref="ContentItem"></see> where this comment applies to.
		/// </summary>
		public virtual ContentItem ContentItem { get; set; }

		/// <summary>
		/// References the <see cref="User"></see> that has made the comment (optional).
		/// </summary>
		public virtual User CreatedBy { get; set; }
      public virtual DateTime CreatedDate { get; set; }
      public virtual User UpdatedBy { get; set; }
      public virtual DateTime? UpdatedDate { get; set; }



		/// <summary>
		/// Creates a new instance of the <see cref="Comment"></see> class.
		/// </summary>
		public Comment()
		{
         this.CommentId = -1;
		}



      #region Equality

      public override bool Equals(object other)
      {
         if (this == other)
            return true;
         Comment c = other as Comment;
         if (c == null)
            return false;
         if (CommentId != c.CommentId)
            return false;
         return true;
      }



      public override int GetHashCode()
      {
         unchecked
         {
            int result;
            result = CommentId.GetHashCode();
            result = 29 * result;
            return result;
         }
      }

      #endregion


	}
}
