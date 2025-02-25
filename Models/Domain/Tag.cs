namespace Bloggie.Web.Models.Domain
{
    public class Tag
    {
        public Guid Id { get; set; }
        
        public string Name { get; set; }

        public string DisplayName { get; set; }
        
        //Maany to many relationshp between Tag and BlogPost
        public ICollection<BlogPost> BlogPosts { get; set; }
    }
}