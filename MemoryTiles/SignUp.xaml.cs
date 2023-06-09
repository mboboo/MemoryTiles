﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;
using System.Xml;
using System.IO;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Text.RegularExpressions;

namespace MemoryTiles
{
    /// <summary>
    /// Interaction logic for SignUp.xaml
    /// </summary>
    public partial class SignUp : System.Windows.Window
    {
        private int currentPhotoIndex = 0;

        private XDocument usersFile = new XDocument(new XElement("users.xml"));

        private List<string> imagePaths = new List<string>()
            {
                "images/airplane.jpg",
                "images/astronaut.jpg",
                "images/ball.jpg",
                "images/beach.jpg",
                "images/butterfly.jpg",
                "images/car.jpg",
                "images/cat.jpg",
                "images/chess.jpg",
                "images/dirtbike.jpg",
                "images/dog.jpg",
                "images/drip.jpg",
                "images/duck.jpg",
                "images/fish.jpg",
                "images/frog.jpg",
                "images/guest.jpg",
                "images/guitar.jpg",
                "images/horses.jpg",
                "images/kick.jpg",
                "images/launch.jpg",
                "images/palmtree.jpg",
                "images/pinkflower.jpg",
                "images/redflower.jpg",
                "images/skater.jpg",
                "images/snowflake.jpg"
            };
        public SignUp()
        {
            InitializeComponent();
            imageProfilePicture.Source = new BitmapImage(new Uri(imagePaths[currentPhotoIndex], UriKind.Relative));
            SpawnInCenterOfScreen();
        }
        private void SpawnInCenterOfScreen()
        {
            Screen screen = Screen.PrimaryScreen;
            System.Drawing.Rectangle workingArea = screen.WorkingArea;

            double left = workingArea.Left + (workingArea.Width - Width) / 2;
            double top = workingArea.Top + (workingArea.Height - Height) / 2;

            WindowStartupLocation = WindowStartupLocation.Manual;
            Left = left;
            Top = top;
        }
        private void buttonNextPhoto_Click(object sender, RoutedEventArgs e)
        {
            currentPhotoIndex += 1;
            if (currentPhotoIndex >= imagePaths.Count)
            {
                currentPhotoIndex = 0;
            }
            imageProfilePicture.Source = new BitmapImage(new Uri(imagePaths[currentPhotoIndex], UriKind.Relative));
        }

        private void buttonPreviousPhoto_Click(object sender, RoutedEventArgs e)
        {
            currentPhotoIndex -= 1;
            if (currentPhotoIndex < 0)
            {
                currentPhotoIndex = imagePaths.Count - 1;
            }
            imageProfilePicture.Source = new BitmapImage(new Uri(imagePaths[currentPhotoIndex], UriKind.Relative));
        }
        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {

            if (existsUsername())
            {
                warningLabel.Content = "Username with this name already exists";
                return;
            }
            if (string.IsNullOrEmpty(newUsernameText.Text) || string.IsNullOrWhiteSpace(newUsernameText.Text) || !Regex.IsMatch(newUsernameText.Text, "^[a-zA-Z0-9]+$"))
            {
                warningLabel.Content = "Invalid username";
                return;
            }
            else
            {
                XmlDocument XmlDocObj = new XmlDocument();
                XmlDocObj.Load("../../users/users.xml");
                XmlNode RootNode = XmlDocObj.SelectSingleNode("users");
                XmlNode userNode = RootNode.AppendChild(XmlDocObj.CreateNode(XmlNodeType.Element, "user", ""));

                userNode.AppendChild(XmlDocObj.CreateNode(XmlNodeType.Element, "name", "")).InnerText = newUsernameText.Text;
                userNode.AppendChild(XmlDocObj.CreateNode(XmlNodeType.Element, "profilepic", "")).InnerText = imagePaths[currentPhotoIndex];

                XmlDocObj.Save("../../users/users.xml");

                MainWindow window = new MainWindow();
                window.Show();

                Close();
            }
        }

        private bool existsUsername()
        {
            XmlReader reader = XmlReader.Create("../../users/users.xml");
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "name")
                {
                    reader.Read();
                    string name = reader.Value;
                    if(newUsernameText.Text.ToString().ToLower() == name.ToLower())
                    {
                        return true;
                    }
                }
            }
            reader.Close();
            return false;
        }
    }
}
