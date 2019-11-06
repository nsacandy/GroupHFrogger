using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FroggerStarter.Model;
using FroggerStarter.View.Sprites;

namespace FroggerStarter.Model
{
    
    class Car:Vehicle
    {
        
        public Car(Vehicle.Heading heading)
        {
            Sprite = new CarSprite();
            
            this.VehicleDirection = heading;
            if (heading.Equals(Vehicle.Heading.Right))
            {
                this.flipHorizontally();
            }
        }
    }
}
