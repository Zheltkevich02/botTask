using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace botTask.DataBase.Tables
{
    public class Comment
    {
        [Key]
        public int IDComment { get; set; }
        public int IDTask { get; set; }
        public int IDUser { get; set; }

        private string TextComment;
        private DateTime DateCreate;

        public string textComment { get { return TextComment; } set { TextComment = value; } }
        public DateTime dateCreate { get { return DateCreate; } set { DateCreate = value; } }

        public Comment() { }

        public Comment(string TextComment, DateTime DateCreate)
        {
            this.TextComment = TextComment;
            this.DateCreate = DateCreate;
        }
    }

}
