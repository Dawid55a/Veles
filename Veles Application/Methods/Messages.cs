using Newtonsoft.Json;
using System;
using System.Windows;
using VelesLibrary.DTOs;

namespace Veles_Application.Methods
{
    //display alerts
    public class Messages
    {
        public static void BadRequest(string jsonResult)
        {
            ResponseDto response = JsonConvert.DeserializeObject<ResponseDto>(jsonResult);

            try
            {
                MessageBox.Show(response.Message, "Error",
                      MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Somethig goes wrong", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
