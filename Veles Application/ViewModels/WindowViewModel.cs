using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Veles_Application.ViewModels
{
    public class WindowViewModel : BaseViewModel
    {
        private BaseViewModel pageViewModel = new LoginViewModel();

        public BaseViewModel PageViewModel
        {
            get { return pageViewModel; }
            set 
            { 
                pageViewModel = value;
                OnPropertyChanged(nameof(PageViewModel));
            }
        }

        public WindowViewModel()
        {
            EventsAggregator.OnPageTransmitted += OnPageRecived;//recive event
        }

        //change page to another
        private void OnPageRecived(BaseViewModel page)
        {
            Task.Run(() =>
            {
                if (page != null) PageViewModel = page;
            });
        }
    }
}
