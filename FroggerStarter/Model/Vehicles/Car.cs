﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FroggerStarter.Model;
using FroggerStarter.View.Sprites;

namespace FroggerStarter.Model.Vehicles
{
    
     public class Car:Vehicle
    {
     
        public Car(Heading heading)
        {
            this.Sprite = new CarSprite();
            this.setHeading(heading);
        }
    }
}
