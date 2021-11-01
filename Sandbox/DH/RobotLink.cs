using System;
using System.Collections.Generic;
using System.Text;
using Leviathan.Math;

using LMath = Leviathan.Math.Math;
namespace Sandbox
{
    public class RobotLink
    {
        public Mat4 Matrix;

        public RobotLink(DHEntry current, DHEntry previous)
        {
            Update(current, previous);
        }
        

        public void Update(DHEntry table_entry, DHEntry previous)
        {
            //scale * rotate * translate
            Matrix = Mat4.Identity;
            Matrix *= Mat4.CreateRotationX(LMath.DegreesToRadians(previous.Alpha));
            Matrix *= Mat4.CreateRotationZ(LMath.DegreesToRadians(table_entry.Theta));
            Matrix *= Mat4.CreateTranslation(table_entry.X_Displacement, 0, table_entry.Z_Displacement);

            //Matrix *= Mat4.(glm.Radians(table_entry.Alpha));
            //Matrix *= mat4.RotateZ(glm.Radians(table_entry.Theta));
            //Matrix *= mat4.Translate(new vec3(table_entry.X_Displacement, 0, table_entry.Z_Displacement));
        }
    }
}
