using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Veles_Application.ViewModels
{
    public class TmpViewModel : BaseViewModel
    {
        public BaseViewModel selectedModel = new GroupViewModel();

        public BaseViewModel SelectedModel
        {
            get { return selectedModel; }
            set { selectedModel = value; }
        }
    }
}
