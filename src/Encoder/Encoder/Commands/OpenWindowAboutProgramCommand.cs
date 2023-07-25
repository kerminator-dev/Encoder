using Encoder.Views;
using EncodingLibrary.Commands;
using System;

namespace Encoder.Commands
{
    internal class OpenWindowAboutProgramCommand : CommandBase
    {
        public override void Execute(object parameter)
        {
            var window = new AboutProgram();

            window.ShowDialog();  
        }
    }
}
