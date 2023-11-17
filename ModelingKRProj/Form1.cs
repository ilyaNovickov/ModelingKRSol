﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ModelingKRProj
{
    public partial class Form1 : Form
    {
        ModelingSystem system;

        public Form1()
        {
            InitializeComponent();

            system = new ModelingSystem();

            propertyGrid.SelectedObject = system;

            propertyGrid.Refresh();

        }

    }
}
