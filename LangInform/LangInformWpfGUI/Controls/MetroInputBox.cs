using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LangInformGUI.Controls
{
    public partial class MetroInputBox : UserControl
    {
        /// <summary>
        /// Shows message window.
        /// </summary>
        /// <param name="mainWindow">Required param. Gets form information</param>
        /// <param name="title">Required param. Message title</param>
        /// <param name="prompt">Required param. Message prompt</param>
        /// <param name="messageButtons">Optional param. Message buttons. Default OK</param>
        /// <returns></returns>
        public static string Show(Window mainWindow, string title, string prompt)
        {
            _mainWindow = mainWindow;
            emptyWindow = new Window();
            emptyWindow.WindowState = mainWindow.WindowState;
            emptyWindow.Height = _mainWindow.Height;
            emptyWindow.Width = mainWindow.Width;
            emptyWindow.Top = mainWindow.Top;
            emptyWindow.Left = mainWindow.Left;
            emptyWindow.WindowStyle = WindowStyle.None;
            emptyWindow.ResizeMode = ResizeMode.NoResize;
            emptyWindow.Background = new SolidColorBrush(Colors.Transparent);
            emptyWindow.AllowsTransparency = true;
            emptyWindow.ShowInTaskbar = false;

            var content = mainWindow.Content;

            if (content is Grid)
            {
                Grid grd = (Grid)content;
                base_grid = new Grid() { Name = "grd_message" };
                RowDefinition rd = new RowDefinition();
                rd.Height = new GridLength(10, GridUnitType.Star);
                ColumnDefinition cd = new ColumnDefinition();
                cd.Width = new GridLength(10, GridUnitType.Star);
                base_grid.RowDefinitions.Add(rd);
                base_grid.ColumnDefinitions.Add(cd);
                CreateMainGrid(emptyWindow, title, prompt);
                base_grid.Children.Add(main_grid);
                emptyWindow.Content = base_grid;
                emptyWindow.ShowDialog();
            }
            return txtInput.Text;
        }

        static Window emptyWindow;

        static Window _mainWindow;

        static MessageResult Result;

        static Grid base_grid;

        static Grid main_grid;

        static Grid background_grid;

        static Grid content_grid;

        /// <summary>
        /// Creating main grid
        /// </summary>
        /// <param name="window"></param>
        /// <param name="title"></param>
        /// <param name="prompt"></param>
        /// <param name="messageButtons"></param>
        static void CreateMainGrid(Window window, string title, string prompt)
        {
            main_grid = new Grid();
            RowDefinition rd = new RowDefinition();
            rd.Height = new GridLength(32, GridUnitType.Star);
            RowDefinition rd1 = new RowDefinition();
            rd1.Height = GridLength.Auto;
            RowDefinition rd2 = new RowDefinition();
            rd2.Height = new GridLength(32, GridUnitType.Star);
            main_grid.RowDefinitions.Add(rd);
            main_grid.RowDefinitions.Add(rd1);
            main_grid.RowDefinitions.Add(rd2);
            CreateBackgroundGrid();
            CreateContentGrid(title, prompt, window);
        }

        /// <summary>
        /// Background grid. Dims the form
        /// </summary>
        static void CreateBackgroundGrid()
        {
            background_grid = new Grid();
            Grid.SetRow(background_grid, 0);
            Grid.SetRowSpan(background_grid, 3);

            background_grid.Background = new SolidColorBrush(Colors.Black);
            background_grid.Opacity = .3;
            main_grid.Children.Add(background_grid);
        }

        static TextBox txtInput;

        /// <summary>
        /// Creating content of the message
        /// </summary>
        /// <param name="_title"></param>
        /// <param name="_message"></param>
        /// <param name="window"></param>
        static void CreateContentGrid(string _title, string _message, Window window)
        {
            content_grid = new Grid();
            content_grid.Background = new SolidColorBrush(Colors.White);
            RowDefinition rd = new RowDefinition();
            rd.Height = new GridLength(40, GridUnitType.Star);
            rd.MinHeight = 40;

            RowDefinition rd1 = new RowDefinition();
            rd1.Height = GridLength.Auto;

            RowDefinition rd2 = new RowDefinition();
            rd2.Height = new GridLength(42, GridUnitType.Star);
            rd2.MinHeight = 42;

            ColumnDefinition cd = new ColumnDefinition();
            cd.Width = new GridLength(50, GridUnitType.Star);

            ColumnDefinition cd1 = new ColumnDefinition();
            cd1.Width = new GridLength(100);

            content_grid.ColumnDefinitions.Add(cd);
            content_grid.ColumnDefinitions.Add(cd1);

            content_grid.RowDefinitions.Add(rd);
            content_grid.RowDefinitions.Add(rd1);
            content_grid.RowDefinitions.Add(rd2);
            //Creating title
            Label title = new Label() { FontSize = 32, FontWeight = FontWeights.Light, VerticalAlignment = VerticalAlignment.Center, VerticalContentAlignment = System.Windows.VerticalAlignment.Center };
            title.Content = _title.ToString();
            Grid.SetRow(title, 0);
            content_grid.Children.Add(title);

            //Creating message
            StackPanel stc = new StackPanel();
            Grid.SetRow(stc, 1);
            txtInput = new TextBox() { Margin = new Thickness(5, 5, 5, 5), FontSize = 16, TextWrapping = TextWrapping.Wrap };
            //message.MaxHeight = _mainWindow.Height - 120;
            txtInput.Text = _message;
            txtInput.TextWrapping = TextWrapping.Wrap;
            ScrollViewer scroll = new ScrollViewer();
            scroll.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            scroll.HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled;
            scroll.Content = txtInput;
            scroll.MaxHeight = _mainWindow.Height - 120;
            stc.Children.Add(scroll);
            content_grid.Children.Add(stc);

            //Creating message Icon
            StackPanel stc1 = new StackPanel();
            Grid.SetRow(stc1, 1);
            Grid.SetColumn(stc1, 1);

            content_grid.Children.Add(stc1);

            //Creating buttons
            //StackPanel stc2 = new StackPanel() { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Right, };
            //Grid.SetRow(stc2, 2);
            //CButton btn_ok = new CButton();
            //btn_ok.Content = "OK";
            //btn_ok.Height = 30;
            //btn_ok.Width = 80;
            //btn_ok.Margin = new Thickness(0, 0, 10, 0);
            //btn_ok.OnClick += CButton_Loaded_1;
            //stc2.Children.Add();
            content_grid.Children.Add(CreateButtons(MessageButtons.OKCancel));
            content_grid.Width = (window.Width * 70 / 100);

            Grid.SetRow(content_grid, 1);
            Grid content_grid1 = new Grid();
            content_grid1.Background = new SolidColorBrush(Colors.White);
            Grid.SetRow(content_grid1, 1);
            main_grid.Children.Add(content_grid1);
            main_grid.Children.Add(content_grid);
        }

        /// <summary>
        /// Creating buttons
        /// </summary>
        /// <param name="messageButton"></param>
        /// <returns></returns>
        static StackPanel CreateButtons(MessageButtons messageButton)
        {
            StackPanel stc2 = new StackPanel() { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Right, };
            Grid.SetRow(stc2, 2);
            if (messageButton == MessageButtons.OKCancel)
            {
                for (int i = 1; i <= 2; i++)
                {
                    CButton btn_ok = new CButton();
                    btn_ok.Content = (i == 1 ? "OK" : "Cancel");
                    btn_ok.Height = 30;
                    btn_ok.Width = 80;
                    btn_ok.Margin = new Thickness(0, 0, 10, 0);
                    btn_ok.OnClick += CButton_OnClick;
                    stc2.Children.Add(btn_ok);
                }
            }
            return stc2;
        }

        /// <summary>
        /// Returns the result and closes the message
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void CButton_OnClick(object sender, RoutedEventArgs e)
        {
            CButton button = (CButton)sender;
            if (button.Content == "OK")
            {
                Result = MessageResult.OK;
            }
            else if (button.Content == "Cancel")
            {
                txtInput.Text = null;
                Result = MessageResult.Cancel;
            }
            emptyWindow.Close();
        }

    }
}
