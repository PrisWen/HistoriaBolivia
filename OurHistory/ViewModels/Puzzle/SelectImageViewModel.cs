using OurHistory.Common;
using OurHistory.Model.Puzzle;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurHistory.ViewModels.Puzzle
{
    public class SelectImageViewModel:BindableBase
    {
        private ObservableCollection<Picture> images;

        public ObservableCollection<Picture> Images
        {
            get
            {
                if (images == null)
                {
                    images = new ObservableCollection<Picture>();
                }
                //LoadImages();
                return images;
            }
            set { images = value; }
        }

        private void LoadImages()
        {
            Images.Add(new Picture() { Name = "Prueba 1", Url = "/Images/Puzzle/imageTest1.jpg" });
            Images.Add(new Picture() { Name = "Prueba 2", Url = "/Images/Puzzle/imageTest1.jpg" });
            Images.Add(new Picture() { Name = "Prueba 3", Url = "/Images/Puzzle/imageTest1.jpg" });
            Images.Add(new Picture() { Name = "Prueba 4", Url = "/Images/Puzzle/imageTest1.jpg" });
        }

        public SelectImageViewModel()
        {
            LoadImages();
        }
    }
}
