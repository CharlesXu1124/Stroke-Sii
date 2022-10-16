using StereoKit;
using System;
using System.Timers;
using System.Collections.Generic;
using StereoKit.Framework;



namespace StereoKitProject1
{

    class Program
    {
        static int counter = 0;
        static int tick = 0;
        static float x, y, z;

        static float start_arrow_z = -0.2f;

        static float start_angle_left = 0;
        static float start_angle_right = 0;

        static float start_arrow_x = 0;

        static int direction = 0; // direction of turning, 1 means right, and -1 means left

        static bool milestone_reached = false;

        /// <summary>
        /// move the arrows 0.005 unit(m) further
        /// </summary>
        static void updateArrowZ()
        {
            start_arrow_z -= 0.005f;
        }

        static void rightTurnArrow()
        {
            start_arrow_z -= 0.010f;
            start_arrow_x += 0.002f;
            start_angle_right -= 1;
        }

        static void leftTurnArrow()
        {
            start_arrow_z -= 0.010f;
            start_arrow_x -= 0.002f;
            start_angle_left += 1;
        }

        static void Main(string[] args)
        {   
            Color _color = Color.Black;

            // Initialize StereoKit
            SKSettings settings = new SKSettings
            {
                appName = "StereoKitProject1",
                assetsFolder = "Assets",
            };


            if (!SK.Initialize(settings))
                Environment.Exit(1);


            // Create assets(cube) used by the app
            Pose cubePose = new Pose(0, 0, -0.5f, Quat.Identity);
            Model cube = Model.FromMesh(
                Mesh.GenerateRoundedCube(Vec3.One * 0.1f, 0.05f),
                Default.MaterialUI);


            Matrix floorTransform = Matrix.TS(0, -1.5f, 0, new Vec3(30, 0.1f, 30));
            Material floorMaterial = new Material(Shader.FromFile("floor.hlsl"));
            floorMaterial.Transparency = Transparency.Blend;

            // arrow model generation
            Model indicator = Model.FromFile("arrow.gltf");

            // Core application loop
            while (SK.Step(() =>
            {

                tick += 1;

                if (tick % 10 == 0)
                {
                    counter += 1;
                }

                Hand rightHand = Input.Hand(Handed.Right);
                Hand leftHand = Input.Hand(Handed.Left);

                Vec3 across = rightHand[FingerId.Index, JointId.Tip].position;
                if (Math.Abs(across.z - start_arrow_z) < 0.07)
                {
                    if (direction == 1)
                    {
                        rightTurnArrow();
                    }
                    if (direction == 0)
                    {
                        updateArrowZ();
                    }
                    if (direction == -1)
                    {
                        leftTurnArrow();
                    }
                    
                }
                if (counter % 5000 == 0) // start turning right
                {
                    direction = 1;
                }
                if (counter % 10000 == 0) // start turning left
                {
                    direction = -1;
                }
                if (direction == 1)
                {
                    indicator.Draw(Matrix.TRS(new Vec3(start_arrow_x, -0.2f, start_arrow_z), new Vec3(0, 180, 0), 0.05f));
                    indicator.Draw(Matrix.TRS(new Vec3(start_arrow_x + 0.05f, -0.2f, start_arrow_z - 0.2f), new Vec3(0, start_angle_right + 160, 0), 0.05f));
                    indicator.Draw(Matrix.TRS(new Vec3(start_arrow_x + 0.15f, -0.2f, start_arrow_z - 0.4f), new Vec3(0, start_angle_right + 140, 0), 0.05f));
                    indicator.Draw(Matrix.TRS(new Vec3(start_arrow_x + 0.35f, -0.2f, start_arrow_z - 0.6f), new Vec3(0, start_angle_right + 120, 0), 0.05f));
                } else if (direction == -1)
                {
                    indicator.Draw(Matrix.TRS(new Vec3(start_arrow_x, -0.2f, start_arrow_z), new Vec3(0, 180, 0), 0.05f));
                    indicator.Draw(Matrix.TRS(new Vec3(start_arrow_x - 0.05f, -0.2f, start_arrow_z - 0.2f), new Vec3(0, start_angle_left + 200, 0), 0.05f));
                    indicator.Draw(Matrix.TRS(new Vec3(start_arrow_x - 0.15f, -0.2f, start_arrow_z - 0.4f), new Vec3(0, start_angle_left + 220, 0), 0.05f));
                    indicator.Draw(Matrix.TRS(new Vec3(start_arrow_x - 0.35f, -0.2f, start_arrow_z - 0.6f), new Vec3(0, start_angle_left + 240, 0), 0.05f));
                } else
                {
                    indicator.Draw(Matrix.TRS(new Vec3(0, -0.2f, start_arrow_z), new Vec3(0, 180, 0), 0.05f));
                    indicator.Draw(Matrix.TRS(new Vec3(0, -0.2f, start_arrow_z - 0.2f), new Vec3(0, 180, 0), 0.05f));
                    indicator.Draw(Matrix.TRS(new Vec3(0, -0.2f, start_arrow_z - 0.4f), new Vec3(0, 180, 0), 0.05f));
                    indicator.Draw(Matrix.TRS(new Vec3(0, -0.2f, start_arrow_z - 0.6f), new Vec3(0, 180, 0), 0.05f));
                }
                
            })) ;
            SK.Shutdown();
        }
        
    }
}
