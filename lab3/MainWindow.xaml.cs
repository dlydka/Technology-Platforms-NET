﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Controls.Primitives;

namespace laboratorium_10
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            DataController.LinqStatements();
            DataController.PerformOperations();


            InitializeComponent();
        }
    }
}
