namespace ReleaseMusicServerlessApi.Models
{
    public class Musics
    {
        public string Name { get; private set; }

        public string Artist { get; private set; }

        public string Date { get; private set; }

        public string Link { get; private set; }

        public string Image { get; private set; }

        public Musics()
        {

        }

        public Musics(string name, string artisit, string date, string link, string image)
        {
            this.Name = name;
            this.Artist = artisit;
            this.Date = date;
            this.Link = link;
            this.Image = image;
        }
    }
}
