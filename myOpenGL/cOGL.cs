using System;
using System.Collections.Generic;
using System.Windows.Forms;

using System.Drawing;



namespace OpenGL
{
    class cOGL
    {

        GLUquadric obj;
        GLUquadric quad;


        public float[] ScrollValue = new float[14];
        public bool checkBox = false;


        float flap = 0;
        public double speed = 0;
        public double speed_inc = 1;
        public double circle_inc = 3;
        public double scale_inc = 15;

        double bubble_up = 0;
        double f = 0;
        double g = 0;
        int circle = 45;
        double hh = 1;
        double swing = 1;
        double bub_Size = 0;


        double updown = 1;
        double ascend = 1;
        bool swingFlg = false;
        bool heightFlg = false;
        bool spirlFlg = false;
        bool flapFlg = false;
        bool bublFlg = false;
        public double lake_size = 100;


        //#define glVertex3f glVertex3f  /* glVertex3f was the short IRIS GL name for
        //                           glVertex3f*/

        Control p;
        int Width;
        int Height;

        public cOGL(Control pb)
        {
            p = pb;
            Width = p.Width;
            Height = p.Height;
            obj = GLU.gluNewQuadric();

            InitializeGL();

            //3 points of ground plane  -  For shadow
            ground[0, 0] = 1;
            ground[0, 1] = 0f;
            ground[0, 2] = 0;

            ground[1, 0] = -1;
            ground[1, 1] = 0f;
            ground[1, 2] = 0;

            ground[2, 0] = 0;
            ground[2, 1] = 0f;
            ground[2, 2] = -1;

            //light position   -   For sun

            pos[0] = light_position[0] = ScrollValue[9];
            pos[1] = light_position[1] = ScrollValue[10];
            pos[2] = light_position[2] = ScrollValue[11];
            pos[3] = light_position[3] = 0;


            light_position_reflected[0] = -ScrollValue[9];
            light_position_reflected[1] = -ScrollValue[10];
            light_position_reflected[2] = -ScrollValue[11];
            light_position_reflected[3] = 0;


            GL.glLightfv(GL.GL_LIGHT0, GL.GL_AMBIENT, light_ambient);
            GL.glLightfv(GL.GL_LIGHT0, GL.GL_DIFFUSE, light_diffuse);
            GL.glLightfv(GL.GL_LIGHT0, GL.GL_SPECULAR, light_specular);
            GL.glLightfv(GL.GL_LIGHT0, GL.GL_POSITION, light_position);

            GL.glLightfv(GL.GL_LIGHT1, GL.GL_AMBIENT, light_ambient);
            GL.glLightfv(GL.GL_LIGHT1, GL.GL_DIFFUSE, light_diffuse);
            GL.glLightfv(GL.GL_LIGHT1, GL.GL_SPECULAR, light_specular);
            GL.glLightfv(GL.GL_LIGHT1, GL.GL_POSITION, light_position_reflected);
            GL.glLightModelfv(GL.GL_LIGHT_MODEL_AMBIENT, lmodel_ambient);


        }

        ~cOGL()
        {
            WGL.wglDeleteContext(m_uint_RC);
        }

        uint m_uint_HWND = 0;

        public uint HWND
        {
            get { return m_uint_HWND; }
        }

        uint m_uint_DC = 0;

        public uint DC
        {
            get { return m_uint_DC; }
        }
        uint m_uint_RC = 0;

        public uint RC
        {
            get { return m_uint_RC; }
        }




        public float zShift = 0.0f;
        public float yShift = 0.0f;
        public float xShift = 0.0f;

        public float zAngle = 0.0f;
        public float yAngle = 0.0f;
        public float xAngle = 0.0f;

        public int intOptionC = 0;


        //Light properties
        float[] light_ambient = { 0.0f, 0.0f, 0.0f, 1.0f };
        float[] light_diffuse = { 1.0f, 1.0f, 1.0f, 1.0f };
        float[] light_specular = { 1.0f, 1.0f, 1.0f, 1.0f };
        float[] light_position = { 0.0f, 100.0f, 0.0f, 0.0f };
        float[] light_position_reflected = { 0.0f, 100.0f, 0.0f, 0.0f };
        float[] lmodel_ambient = { 0.4f, 0.4f, 0.4f, 1.0f };


        public float sun = 0;
        public float planet = 0;
        float[] cubeXform = new float[16];
        float[] planeCoeff = { 1, 1, 1, 1 };
        float[,] ground = new float[3, 3];
        public float[] pos = new float[4];

        public void Drawlake()
        {
            GL.glEnable(GL.GL_BLEND);

            float[] lake_ambuse = { 0.0117f, 0.4296f, 0.9562f, 0.0f };
            GL.glMaterialfv(GL.GL_FRONT_AND_BACK, GL.GL_AMBIENT_AND_DIFFUSE, lake_ambuse);

            GL.glPushMatrix();

            GL.glColor4f(0.0117f, 0.4296f, 0.6562f, 0.5f);
            GL.glRotatef(90, 1, 0, 0);


            GLU.gluDisk(obj, 0, lake_size, 30, 30);// size of lake

            GL.glPopMatrix();
        }

        public void drawSun()
        {
            GL.glPushMatrix();
            GL.glColor3d(1, 1, 1);
            GL.glEnable(GL.GL_TEXTURE_2D);
            GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[6]);


            quad = GLU.gluNewQuadric();
            GLU.gluQuadricTexture(quad, 40);

            GL.glTranslatef(pos[0], pos[1], pos[2]);

            // rotating sun
            GL.glRotatef((float)sun, 0.0f, 1.0f, 0.0f);

            GLU.gluSphere(quad, 10, 50, 50);



            GL.glEnable(GL.GL_TEXTURE_2D);

            //sun lines
            for (int s = 0; s < 16; s++)
            {
                GL.glRotated(22.5, 0, 0, 1);
                GL.glBegin(GL.GL_LINES);
                GL.glVertex3d(0, 18, 0);
                GL.glVertex3d(0, -18, 0);
                GL.glEnd();
                GL.glBegin(GL.GL_LINES);
                GL.glVertex3d(18, 0, 0);
                GL.glVertex3d(-18, 0, 0);
                GL.glEnd();
                GL.glBegin(GL.GL_LINES);
                GL.glVertex3d(0, 0, 18);
                GL.glVertex3d(0, 0, -18);
                GL.glEnd();
            }
            for (int s = 0; s < 16; s++)
            {
                GL.glRotated(22.5, 0, 1, 0);
                GL.glBegin(GL.GL_LINES);
                GL.glVertex3d(0, 18, 0);
                GL.glVertex3d(0, -18, 0);
                GL.glEnd();
                GL.glBegin(GL.GL_LINES);
                GL.glVertex3d(18, 0, 0);
                GL.glVertex3d(-18, 0, 0);
                GL.glEnd();
                GL.glBegin(GL.GL_LINES);
                GL.glVertex3d(0, 0, 18);
                GL.glVertex3d(0, 0, -18);
                GL.glEnd();
            }
            for (int s = 0; s < 16; s++)
            {
                GL.glRotated(22.5, 1, 0, 0);
                GL.glBegin(GL.GL_LINES);
                GL.glVertex3d(0, 18, 0);
                GL.glVertex3d(0, -18, 0);
                GL.glEnd();
                GL.glBegin(GL.GL_LINES);
                GL.glVertex3d(18, 0, 0);
                GL.glVertex3d(-18, 0, 0);
                GL.glEnd();
                GL.glBegin(GL.GL_LINES);
                GL.glVertex3d(0, 0, 18);
                GL.glVertex3d(0, 0, -18);
                GL.glEnd();
            }


            GL.glPopMatrix();

            sun += 0.44f;
        }

        public void drawaxe()
        {

            GL.glBegin(GL.GL_LINES);
            //x  RED
            GL.glColor3f(1.0f, 0.0f, 0.0f);
            GL.glVertex3f(0.0f, 0.0f, 0.0f);
            GL.glVertex3f(150.0f, 0.0f, 0.0f);
            //y  GREEN 
            GL.glColor3f(0.0f, 1.0f, 0.0f);
            GL.glVertex3f(0.0f, 0.0f, 0.0f);
            GL.glVertex3f(0.0f, 150.0f, 0.0f);
            //z  BLUE
            GL.glColor3f(0.0f, 0.0f, 1.0f);
            GL.glVertex3f(0.0f, 0.0f, 0.0f);
            GL.glVertex3f(0.0f, 0.0f, 150.0f);
            GL.glEnd();
        }

        public void Draw()
        {

            if (m_uint_DC == 0 || m_uint_RC == 0)
                return;

            GL.glClear(GL.GL_COLOR_BUFFER_BIT | GL.GL_DEPTH_BUFFER_BIT | GL.GL_STENCIL_BUFFER_BIT);

            //TRIVIAL
            GL.glViewport(0, 0, Width, Height);
            GL.glLoadIdentity();
            GL.glEnable(GL.GL_NORMALIZE);

            GLU.gluLookAt(ScrollValue[0], ScrollValue[1], ScrollValue[2],
                           ScrollValue[3], ScrollValue[4], ScrollValue[5],
                           ScrollValue[6], ScrollValue[7], ScrollValue[8]);



            pos[0] = light_position[0] = ScrollValue[9];
            pos[1] = light_position[1] = ScrollValue[10];
            pos[2] = light_position[2] = ScrollValue[11];
            pos[3] = light_position[3] = 0;

            light_position_reflected[0] = -ScrollValue[9];
            light_position_reflected[1] = -ScrollValue[10];
            light_position_reflected[2] = -ScrollValue[11];
            light_position[3] = 0;



            GL.glLightfv(GL.GL_LIGHT0, GL.GL_POSITION, light_position);
            GL.glEnable(GL.GL_LIGHT0);

            GL.glLightfv(GL.GL_LIGHT1, GL.GL_POSITION, light_position);
            GL.glEnable(GL.GL_LIGHT1);




            //Looking angle
            //drawaxe();
            GL.glTranslatef(0.0f, -120.0f, -380.0f);//how far from the lake,-far +close.
            GL.glRotatef(28, 1.0f, 0, 0);//look at lake angle ,+up -down
            GL.glRotatef(15, 0, 1.0f, 0);



            GL.glRotatef(xAngle, 1.0f, 0.0f, 0.0f);
            GL.glRotatef(yAngle, 0.0f, 1.0f, 0.0f);
            GL.glRotatef(zAngle, 0.0f, 0.0f, 1.0f);
            GL.glTranslatef(xShift, yShift, zShift);


            /*
             * 
             * Reflection drawing section 
             * 
             */

            GL.glPushMatrix();

            GL.glEnable(GL.GL_BLEND);
            GL.glBlendFunc(GL.GL_SRC_ALPHA, GL.GL_ONE_MINUS_SRC_ALPHA);

            //draw only to STENCIL buffer
            GL.glEnable(GL.GL_STENCIL_TEST);
            GL.glStencilOp(GL.GL_REPLACE, GL.GL_REPLACE, GL.GL_REPLACE);
            GL.glStencilFunc(GL.GL_ALWAYS, 1, 0xFFFFFFFF);
            GL.glColorMask((byte)GL.GL_FALSE, (byte)GL.GL_FALSE, (byte)GL.GL_FALSE, (byte)GL.GL_FALSE);
            GL.glDisable(GL.GL_DEPTH_TEST);

            Drawlake();//Draw lake where we want to see reflection

            // restore regular seascendings
            GL.glColorMask((byte)GL.GL_TRUE, (byte)GL.GL_TRUE, (byte)GL.GL_TRUE, (byte)GL.GL_TRUE);
            GL.glEnable(GL.GL_DEPTH_TEST);

            // reflection is drawn only where STENCIL buffer value equal to 1
            GL.glStencilFunc(GL.GL_EQUAL, 1, 0xFFFFFFFF);
            GL.glStencilOp(GL.GL_KEEP, GL.GL_KEEP, GL.GL_KEEP);


            /*
             * draw reflected scene 
             */

            GL.glScalef(1, -1, 1); //swap axes down 

            GL.glEnable(GL.GL_LIGHTING);

            GL.glPushMatrix();


            drawWaterSurfaceTextured();

            for (int p = 0; p < 32; p++)
            {
                GL.glTranslated(200.0f, 0.0f, 0.0f);
                GL.glRotated(swing * 2, 0, 0, 1);
                drawSeaWead();
                GL.glRotated(-swing * 2, 0, 0, 1);
                GL.glTranslated(-200.0f, 0.0f, 0.0f);
                GL.glRotated(11.25, 0, 1, 0);
            }


            for (int p = 0; p < 32; p++)
            {
                GL.glTranslated(220.0f, 0.0f, 0.0f);
                GL.glRotated(swing * 3, 0, 0, 1);
                drawSeaWead();
                GL.glRotated(-swing * 3, 0, 0, 1);
                GL.glTranslated(-220.0f, 0.0f, 0.0f);
                GL.glRotated(11.25, 0, 1, 0);
            }

            for (int p = 0; p < 64; p++)
            {
                GL.glTranslated(240.0f, 0.0f, 0.0f);
                GL.glRotated(swing * 2, 0, 0, 1);
                drawSeaWead();
                GL.glRotated(-swing * 2, 0, 0, 1);
                GL.glTranslated(-240.0f, 0.0f, 0.0f);
                GL.glRotated(5.625, 0, 1, 0);
            }

            for (int p = 0; p < 32; p++)
            {
                GL.glTranslated(260.0f, 0.0f, 0.0f);
                GL.glRotated(swing * 4, 0, 0, 1);
                drawSeaWead();
                GL.glRotated(-swing * 4, 0, 0, 1);
                GL.glTranslated(-260.0f, 0.0f, 0.0f);
                GL.glRotated(11.25, 0, 1, 0);
            }

            GL.glPopMatrix();

            GL.glPushMatrix();
            //end lake//

            //nofar flower//

            GL.glTranslated(0, 251, 0);
            GL.glTranslated(0, 0, circle * 0.5);

            GL.glRotated(sun, 0, 1, 0);



            //   drawNofarleaf //center
            for (int k = 0; k < 8; k++) //leafs
            {
                GL.glTranslated(12, 0, 0);
                drawNofarleaf();         //Base of flower

                GL.glTranslated(-12, 0, 0);
                GL.glRotated(45, 0, 1, 0);
            }

            GL.glTranslated(0, 0, -circle * 0.5);

            GL.glTranslated(0, -251, 0);
            GL.glPopMatrix();
            GL.glTranslated(0, 0, circle * 0.5);

            drawNofar();
            GL.glTranslated(0, 0, -circle * 0.5);

           //end nofar//
            /////////

            /////////

            DrawTexturedCube();
         
            drawfish();

            drawSun();

            GL.glPopMatrix(); 


            //End Reflection draw area


            GL.glStencilFunc(GL.GL_NOTEQUAL, 1, 0xFFFFFFFF);
            GL.glStencilOp(GL.GL_KEEP, GL.GL_KEEP, GL.GL_KEEP);

            GL.glDepthMask((byte)GL.GL_FALSE);
            GL.glDepthMask((byte)GL.GL_TRUE);

            drawFloorTextured();

            GL.glDisable(GL.GL_STENCIL_TEST);



            /*
             * 
             * Paint main scene - 
             * 
             */

            DrawTexturedCube();

            drawSun();
            GL.glEnable(GL.GL_LIGHTING);
            
            drawfish();

            //draw Sea Wead
            GL.glPushMatrix();
            for (int p = 0; p < 32; p++)
            {
                GL.glTranslated(200.0f, 0.0f, 0.0f);
                GL.glRotated(swing*2, 0, 0, 1);
                drawSeaWead();
                GL.glRotated(-swing*2, 0, 0, 1);

                GL.glTranslated(-200.0f, 0.0f, 0.0f);

                GL.glRotated(11.25, 0, 1, 0);
            }
            for (int p = 0; p < 32; p++)
            {
                GL.glTranslated(220.0f, 0.0f, 0.0f);
                GL.glRotated(swing * 3, 0, 0, 1);
                drawSeaWead();
                GL.glRotated(-swing * 3, 0, 0, 1);

                GL.glTranslated(-220.0f, 0.0f, 0.0f);

                GL.glRotated(11.25, 0, 1, 0);
            }
            for (int p = 0; p < 64; p++)
            {
                GL.glTranslated(240.0f, 0.0f, 0.0f);
                GL.glRotated(swing * 2, 0, 0, 1);
                drawSeaWead();
                GL.glRotated(-swing * 2, 0, 0, 1);

                GL.glTranslated(-240.0f, 0.0f, 0.0f);

                GL.glRotated(5.625, 0, 1, 0);
            }
            for (int p = 0; p < 32; p++)
            {
                GL.glTranslated(260.0f, 0.0f, 0.0f);
                GL.glRotated(swing * 4, 0, 0, 1);
                drawSeaWead();
                GL.glRotated(-swing * 4, 0, 0, 1);

                GL.glTranslated(-260.0f, 0.0f, 0.0f);

                GL.glRotated(11.25, 0, 1, 0);
            }

            GL.glPopMatrix();
            GL.glPushMatrix();


            GL.glTranslated(0, 251, 0);
            GL.glTranslated(0, 0, circle * 0.5);

            GL.glRotated(sun, 0, 1, 0);


            //   drawNofarleaf();


            for (int k = 0; k < 8; k++) //leafs
            {
                GL.glTranslated(12, 0, 0);
                drawNofarleaf();

                GL.glTranslated(-12, 0, 0);
                GL.glRotated(45, 0, 1, 0);
            }

            GL.glTranslated(0, 0, -circle * 0.5);

            GL.glTranslated(0, -251, 0);
            GL.glPopMatrix();
            GL.glTranslated(0, 0, circle * 0.5);

            drawNofar();
            GL.glTranslated(0, 0, -circle*0.5);


            /*
             * 
             * Draw shadows
             * 
             */

            GL.glDisable(GL.GL_LIGHTING);


            GL.glPushMatrix();

            MakeShadowMatrix(ground);
            GL.glMultMatrixf(cubeXform);

            GL.glShadeModel(GL.GL_FLAT);
            GL.glColor3d(0, 0, 0);//black

            //Draw sea wead shade  -  11.25*32=360
            for (int t = 0; t < 32; t++)
            {
                GL.glTranslated(200.0f, 0.0f, 0.0f);
                GL.glRotated(swing * 2, 0, 0, 1);
                drawSeaWeadShade();
                GL.glRotated(-swing * 2, 0, 0, 1);
                GL.glTranslated(-200.0f, 0.0f, 0.0f);
                GL.glRotated(11.25, 0, 1, 0);
            }


            for (int t = 0; t < 32; t++)
            {
                GL.glTranslated(220.0f, 0.0f, 0.0f);
                GL.glRotated(swing * 3, 0, 0, 1);
                drawSeaWeadShade();
                GL.glRotated(-swing * 3, 0, 0, 1);
                GL.glTranslated(-220.0f, 0.0f, 0.0f);
                GL.glRotated(11.25, 0, 1, 0);
            }

            for (int t = 0; t < 64; t++)
            {
                GL.glTranslated(240.0f, 0.0f, 0.0f);
                GL.glRotated(swing * 2, 0, 0, 1);
                drawSeaWeadShade();
                GL.glRotated(-swing * 2, 0, 0, 1);
                GL.glTranslated(-240.0f, 0.0f, 0.0f);
                GL.glRotated(5.625, 0, 1, 0);
            }

            for (int t = 0; t < 32; t++)
            {
                GL.glTranslated(260.0f, 0.0f, 0.0f);
                GL.glRotated(swing * 4, 0, 0, 1);
                drawSeaWeadShade();
                GL.glRotated(-swing * 4, 0, 0, 1);
                GL.glTranslated(-260.0f, 0.0f, 0.0f);
                GL.glRotated(11.25, 0, 1, 0);
            }

            drawfishShade();

            GL.glPushMatrix();

            //Nofars shadows
            //Go up for flower
            GL.glTranslated(0, 251, 0);
            GL.glTranslated(0, 0, circle * 0.5);
            //rotate flower
            GL.glRotated(sun, 0, 1, 0);



            for (int k = 0; k < 8; k++) //leafs
            {
                GL.glTranslated(12, 0, 0);
                drawNofarleafShade();

                GL.glTranslated(-12, 0, 0);
                GL.glRotated(45, 0, 1, 0);
            }

            GL.glTranslated(0, 0, -circle * 0.5);

            GL.glTranslated(0, -251, 0);
            GL.glPopMatrix();
            GL.glTranslated(0, 0, circle * 0.5);

            drawNofarShade();
            GL.glTranslated(0, 0, -circle * 0.5);


            GL.glPopMatrix();

            drawWaterSurfaceTextured();


            GL.glFlush();
            WGL.wglSwapBuffers(m_uint_DC);
        }





        /*
        * 
        * SHADOWS FUNCS
        * 
        */
        void ReduceToUnit(float[] vector)
        {
            float length;

            // Calculate the length of the vector		
            length = (float)Math.Sqrt((vector[0] * vector[0]) +
                                (vector[1] * vector[1]) +
                                (vector[2] * vector[2]));

            // Keep the program from blowing up by providing an exceptable
            // value for vectors that may calculated too close to zero.
            if (length == 0.0f)
                length = 1.0f;

            // Dividing each element by the length will result in a
            // unit normal vector.
            vector[0] /= length;
            vector[1] /= length;
            vector[2] /= length;
        }

        const int x = 0;
        const int y = 1;
        const int z = 2;

        // Points p1, p2, & p3 specified in counter clock-wise order
        void calcNormal(float[,] v, float[] outp)
        {
            float[] v1 = new float[3];
            float[] v2 = new float[3];

            // Calculate two vectors from the three points
            v1[x] = v[0, x] - v[1, x];
            v1[y] = v[0, y] - v[1, y];
            v1[z] = v[0, z] - v[1, z];

            v2[x] = v[1, x] - v[2, x];
            v2[y] = v[1, y] - v[2, y];
            v2[z] = v[1, z] - v[2, z];

            // Take the cross product of the two vectors to get
            // the normal vector which will be stored in out
            outp[x] = Math.Abs(v1[y] * v2[z] - v1[z] * v2[y]);
            outp[y] = Math.Abs(v1[z] * v2[x] - v1[x] * v2[z]);//Abs added..
            outp[z] = Math.Abs(v1[x] * v2[y] - v1[y] * v2[x]);

            // Normalize the vector (shorten length to one)
            ReduceToUnit(outp);
        }

        // Creates a shadow projection matrix out of the plane equation
        // coefficients and the position of the light. The return value is stored
        // in cubeXform[,]
        void MakeShadowMatrix(float[,] points)
        {
            float dot;

            // Find the plane equation coefficients
            // Find the first three coefficients the same way we
            // find a normal.
            calcNormal(points, planeCoeff);

            // Find the last coefficient by back substitutions
            planeCoeff[3] = -(
                (planeCoeff[0] * points[2, 0]) + (planeCoeff[1] * points[2, 1]) +
                (planeCoeff[2] * points[2, 2]));


            // Dot product of plane and light position
            dot = planeCoeff[0] * pos[0] +
                    planeCoeff[1] * pos[1] +
                    planeCoeff[2] * pos[2] +
                    planeCoeff[3];

            // Now do the projection
            // First column
            cubeXform[0] = dot - pos[0] * planeCoeff[0];
            cubeXform[4] = 0.0f - pos[0] * planeCoeff[1];
            cubeXform[8] = 0.0f - pos[0] * planeCoeff[2];
            cubeXform[12] = 0.0f - pos[0] * planeCoeff[3];

            // Second column
            cubeXform[1] = 0.0f - pos[1] * planeCoeff[0];
            cubeXform[5] = dot - pos[1] * planeCoeff[1];
            cubeXform[9] = 0.0f - pos[1] * planeCoeff[2];
            cubeXform[13] = 0.0f - pos[1] * planeCoeff[3];

            // Third Column
            cubeXform[2] = 0.0f - pos[2] * planeCoeff[0];
            cubeXform[6] = 0.0f - pos[2] * planeCoeff[1];
            cubeXform[10] = dot - pos[2] * planeCoeff[2];
            cubeXform[14] = 0.0f - pos[2] * planeCoeff[3];

            // Fourth Column
            cubeXform[3] = 0.0f - pos[3] * planeCoeff[0];
            cubeXform[7] = 0.0f - pos[3] * planeCoeff[1];
            cubeXform[11] = 0.0f - pos[3] * planeCoeff[2];
            cubeXform[15] = dot - pos[3] * planeCoeff[3];
        }


        protected virtual void InitializeGL()
        {
            m_uint_HWND = (uint)p.Handle.ToInt32();
            m_uint_DC = WGL.GetDC(m_uint_HWND);

            // Not doing the following WGL.wglSwapBuffers() on the DC will
            // result in a failure to subsequently create the RC.
            WGL.wglSwapBuffers(m_uint_DC);

            WGL.PIXELFORMATDESCRIPTOR pfd = new WGL.PIXELFORMATDESCRIPTOR();
            WGL.ZeroPixelDescriptor(ref pfd);
            pfd.nVersion = 1;
            pfd.dwFlags = (WGL.PFD_DRAW_TO_WINDOW | WGL.PFD_SUPPORT_OPENGL | WGL.PFD_DOUBLEBUFFER);
            pfd.iPixelType = (byte)(WGL.PFD_TYPE_RGBA);
            pfd.cColorBits = 32;
            pfd.cDepthBits = 32;
            pfd.iLayerType = (byte)(WGL.PFD_MAIN_PLANE);

            int pixelFormatIndex = 0;
            pixelFormatIndex = WGL.ChoosePixelFormat(m_uint_DC, ref pfd);
            if (pixelFormatIndex == 0)
            {
                MessageBox.Show("Unable to retrieve pixel format");
                return;
            }

            if (WGL.SetPixelFormat(m_uint_DC, pixelFormatIndex, ref pfd) == 0)
            {
                MessageBox.Show("Unable to set pixel format");
                return;
            }
            //Create rendering context
            m_uint_RC = WGL.wglCreateContext(m_uint_DC);
            if (m_uint_RC == 0)
            {
                MessageBox.Show("Unable to get rendering context");
                return;
            }
            if (WGL.wglMakeCurrent(m_uint_DC, m_uint_RC) == 0)
            {
                MessageBox.Show("Unable to make rendering context current");
                return;
            }

            initRenderinspeedL();
        }

        public void OnResize()
        {
            Width = p.Width;
            Height = p.Height;
            GL.glViewport(0, 0, Width, Height);
            Draw();
        }

        protected virtual void initRenderinspeedL()
        {
            if (m_uint_DC == 0 || m_uint_RC == 0)
                return;
            if (this.Width == 0 || this.Height == 0)
                return;
            //GL.glClearColor(0.5f, 0.9f, 1.0f, 1.0f);

            GL.glClearColor(1.0f, 1.0f, 1.0f, 1.0f);
            GL.glEnable(GL.GL_DEPTH_TEST);
            GL.glDepthFunc(GL.GL_LEQUAL);

            GL.glViewport(0, 0, this.Width, this.Height);
            GL.glMatrixMode(GL.GL_PROJECTION);
            GL.glLoadIdentity();


            GL.glShadeModel(GL.GL_SMOOTH);

            GLU.gluPerspective(60, (float)Width / (float)Height, 0.45f, 1000.0f);

            GenerateTextures(1);

            GL.glMatrixMode(GL.GL_MODELVIEW);
            GL.glLoadIdentity();

        }

        public uint[] Textures = new uint[7];
        public void GenerateTextures(int texture)
        {
            GL.glBlendFunc(GL.GL_SRC_ALPHA, GL.GL_ONE_MINUS_SRC_ALPHA);
            GL.glGenTextures(7, Textures);

          
            string[] imagesName ={ "posy.jpg","posx.jpg",
                                    "posy.jpg","posx.jpg","top1.bmp","water.jpg","sun.jpg",};

            for (int i = 0; i < 7; i++)
            {
                Bitmap image = new Bitmap(imagesName[i]);
                image.RotateFlip(RotateFlipType.RotateNoneFlipY); //Y axis in Windows is directed downwards, while in OpenGL-upwards
                System.Drawing.Imaging.BitmapData bitmapdata;
                Rectangle rect = new Rectangle(0, 0, image.Width, image.Height);

                bitmapdata = image.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

                GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[i]);
                //2D for XYZ
                GL.glTexImage2D(GL.GL_TEXTURE_2D, 0, (int)GL.GL_RGB8, image.Width, image.Height,
                                                              0, GL.GL_BGR_EXT, GL.GL_UNSIGNED_byte, bitmapdata.Scan0);
                GL.glTexParameteri(GL.GL_TEXTURE_2D, GL.GL_TEXTURE_MIN_FILTER, (int)GL.GL_LINEAR);
                GL.glTexParameteri(GL.GL_TEXTURE_2D, GL.GL_TEXTURE_MAG_FILTER, (int)GL.GL_LINEAR);

                image.UnlockBits(bitmapdata);
                image.Dispose();
            }
        }

        void drawFloorTextured()
        {
            GL.glEnable(GL.GL_TEXTURE_2D);
            GL.glDisable(GL.GL_BLEND);
            GL.glColor3d(1, 1, 1);
            GL.glDisable(GL.GL_LIGHTING);
            GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[5]);
            GL.glBegin(GL.GL_QUADS);
            GL.glNormal3f(0, 1, 0);
            GL.glTexCoord2f(0.0f, 0.0f); GL.glVertex3f(-300, -0.01f, 300);
            GL.glTexCoord2f(1.0f, 0.0f); GL.glVertex3f(300, -0.01f, 300);
            GL.glTexCoord2f(1.0f, 1.0f); GL.glVertex3f(300, -0.01f, -300);
            GL.glTexCoord2f(0.0f, 1.0f); GL.glVertex3f(-300, -0.01f, -300);
            GL.glEnd();
            GL.glDisable(GL.GL_TEXTURE_2D);
            GL.glEnable(GL.GL_BLEND);
            GL.glEnable(GL.GL_LIGHTING);

        }

    

        void drawWaterSurfaceTextured()
        {
            GL.glEnable(GL.GL_TEXTURE_2D);
            GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[5]);
            GL.glDisable(GL.GL_LIGHTING);

            GL.glColor4d(1, 1, 1, 0.5);
            GL.glBegin(GL.GL_QUADS);
            GL.glNormal3f(0, 1, 0);
            GL.glTexCoord2f(0.0f, 0.0f); GL.glVertex3f(-300, 248.0f +(float)swing* 0.48f, 300);
            GL.glTexCoord2f(1.0f, 0.0f); GL.glVertex3f(300, 248.0f - (float)swing * 0.48f, 300);
            GL.glTexCoord2f(1.0f, 1.0f); GL.glVertex3f(300, 248.0f + (float)swing * 0.48f, -300);
            GL.glTexCoord2f(0.0f, 1.0f); GL.glVertex3f(-300, 248.0f - (float)swing * 0.48f, -300);
            GL.glEnd();
            GL.glEnable(GL.GL_LIGHTING);
            GL.glDisable(GL.GL_TEXTURE_2D);


        }
        

        void DrawTexturedCube()
        {
            GL.glEnable(GL.GL_TEXTURE_2D);
            GL.glDisable(GL.GL_BLEND);
            GL.glColor3d(1, 1, 1);
            GL.glDisable(GL.GL_LIGHTING);
            // front
            GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[0]);
            GL.glBegin(GL.GL_QUADS);
            GL.glTexCoord2f(0.0f, 0.0f); GL.glVertex3f(300.0f, -0.01f, 300.0f);
            GL.glTexCoord2f(1.0f, 0.0f); GL.glVertex3f(-300.0f, -0.01f, 300.0f);
            GL.glTexCoord2f(1.0f, 1.0f); GL.glVertex3f(-300.0f, 300.0f, 300.0f);
            GL.glTexCoord2f(0.0f, 1.0f); GL.glVertex3f(300.0f, 300.0f, 300.0f);
            GL.glEnd();
            // back
            GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[1]);
            GL.glBegin(GL.GL_QUADS);
            GL.glTexCoord2f(0.0f, 0.0f); GL.glVertex3f(-300.0f, -0.01f, -300.0f);
            GL.glTexCoord2f(1.0f, 0.0f); GL.glVertex3f(300.0f, -0.01f, -300.0f);
            GL.glTexCoord2f(1.0f, 1.0f); GL.glVertex3f(300.0f, 300.0f, -300.0f);
            GL.glTexCoord2f(0.0f, 1.0f); GL.glVertex3f(-300.0f, 300.0f, -300.0f);
            GL.glEnd();
            // left
            GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[2]);
            GL.glBegin(GL.GL_QUADS);
            GL.glTexCoord2f(0.0f, 0.0f); GL.glVertex3f(-300.0f, -0.01f, 300.0f);
            GL.glTexCoord2f(1.0f, 0.0f); GL.glVertex3f(-300.0f, -0.01f, -300.0f);
            GL.glTexCoord2f(1.0f, 1.0f); GL.glVertex3f(-300.0f, 300.0f, -300.0f);
            GL.glTexCoord2f(0.0f, 1.0f); GL.glVertex3f(-300.0f, 300.0f, 300.0f);
            GL.glEnd();
            // right
            GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[3]);
            GL.glBegin(GL.GL_QUADS);
            GL.glTexCoord2f(0.0f, 0.0f); GL.glVertex3f(300.0f, -0.01f, -300.0f);
            GL.glTexCoord2f(1.0f, 0.0f); GL.glVertex3f(300.0f, -0.01f, 300.0f);
            GL.glTexCoord2f(1.0f, 1.0f); GL.glVertex3f(300.0f, 300.0f, 300.0f);
            GL.glTexCoord2f(0.0f, 1.0f); GL.glVertex3f(300.0f, 300.0f, -300.0f);
            GL.glEnd();
            // top
            GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[4]);
            GL.glBegin(GL.GL_QUADS);
            GL.glTexCoord2f(0.0f, 0.0f); GL.glVertex3f(-300.0f, 300.0f, -300.0f);
            GL.glTexCoord2f(1.0f, 0.0f); GL.glVertex3f(300.0f, 300.0f, -300.0f);
            GL.glTexCoord2f(1.0f, 1.0f); GL.glVertex3f(300.0f, 300.0f, 300.0f);
            GL.glTexCoord2f(0.0f, 1.0f); GL.glVertex3f(-300.0f, 300.0f, 300.0f);
            GL.glEnd();

            GL.glDisable(GL.GL_TEXTURE_2D);
            GL.glEnable(GL.GL_BLEND);
        }


        //Base of flower
        public void drawNofarleaf()
        {
            float[] nofarBase_ambuse = { 0.00f, 0.5296f, 0.2562f, 1.0f };

            GL.glMaterialfv(GL.GL_FRONT_AND_BACK, GL.GL_AMBIENT_AND_DIFFUSE, nofarBase_ambuse);

            GL.glPushMatrix();
            GL.glColor4f(0.0117f, 0.8296f, 0.2562f, 1.0f);
            GL.glRotatef(90, 1, 0, 0);
            GLU.gluDisk(obj, 0, lake_size / 6, 30, 30);// size of lake
            GL.glPopMatrix();
        }

        public void drawNofarleafShade()
        {
            float[] nofarBase_ambuse = { 0.00f, 0.0f, 0.0f, 1.0f };

            GL.glMaterialfv(GL.GL_FRONT_AND_BACK, GL.GL_AMBIENT_AND_DIFFUSE, nofarBase_ambuse);

            GL.glPushMatrix();
            GL.glColor4f(0.00f, 0.0f, 0.0f, 1.0f);
            GL.glRotatef(90, 1, 0, 0);
            GLU.gluDisk(obj, 0, lake_size / 6, 30, 30);// size of lake
            GL.glPopMatrix();
        }

        public void drawNofarFlower()
        {

            GL.glPushMatrix();

            GL.glRotatef(-90, 1, 0, 0);
            GL.glRotatef(42, 1, 0, 0);


            float[] flower_ambuse = { 0.183f, 0.514f, 0.213f, 1.0f };
            GL.glMaterialfv(GL.GL_FRONT_AND_BACK, GL.GL_AMBIENT_AND_DIFFUSE, flower_ambuse);

            //// Flower-Top
            ///

            GL.glScaled(lake_size / 160, lake_size / 160, lake_size / 160);
            GL.glBegin(GL.GL_TRIANGLE_STRIP);
            GL.glNormal3d(0, 0, 1);

            GL.glVertex3f(0f, 0.0f, 0.0f);//1
            GL.glVertex3f(-5.0f, 2.0f, -10.0f);//2
            GL.glVertex3f(0.0f, 2.0f, 13.0f);//3
            GL.glEnd();

            GL.glBegin(GL.GL_TRIANGLE_STRIP);
            GL.glNormal3d(0, 0, 1);

            GL.glVertex3f(0f, 0.0f, 0.0f);//1
            GL.glVertex3f(5.0f, 2.0f, -10.0f);//2
            GL.glVertex3f(0.0f, 2.0f, 13.0f);//3
            GL.glEnd();



            GL.glBegin(GL.GL_TRIANGLE_STRIP);
            GL.glNormal3d(0, 0, 1);

            GL.glVertex3f(-5.0f, 4.0f, -10.0f);//1
            GL.glVertex3f(0.0f, 4.0f, 13.0f);//2
            GL.glVertex3f(0.0f, 0.0f, 22.0f);//3
            GL.glEnd();

            GL.glBegin(GL.GL_TRIANGLE_STRIP);
            GL.glNormal3d(0, 0, 1);

            GL.glVertex3f(5.0f, 4.0f, -10.0f);//1
            GL.glVertex3f(0.0f, 4.0f, 13.0f);//2
            GL.glVertex3f(0.0f, 0.0f, 22.0f);//3
            GL.glEnd();

            GL.glRotatef(-8, 1, 0, 0);

            //// Flower-Top2
            ///

            GL.glScaled(lake_size / 80, lake_size / 80, lake_size / 80);
            GL.glBegin(GL.GL_TRIANGLE_STRIP);
            GL.glNormal3d(0, 0, 1);

            GL.glVertex3f(0f, 0.0f, 0.0f);//1
            GL.glVertex3f(-5.0f, 2.0f, 10.0f);//2
            GL.glVertex3f(0.0f, 2.0f, 13.0f);//3
            GL.glEnd();

            GL.glBegin(GL.GL_TRIANGLE_STRIP);
            GL.glNormal3d(0, 0, 1);

            GL.glVertex3f(0f, 0.0f, 0.0f);//1
            GL.glVertex3f(5.0f, 2.0f, 10.0f);//2
            GL.glVertex3f(0.0f, 2.0f, 13.0f);//3
            GL.glEnd();



            GL.glBegin(GL.GL_TRIANGLE_STRIP);
            GL.glNormal3d(0, 0, 1);

            GL.glVertex3f(-5.0f, 4.0f, 10.0f);//1
            GL.glVertex3f(0.0f, 4.0f, 13.0f);//2
            GL.glVertex3f(0.0f, 0.0f, 22.0f);//3
            GL.glEnd();

            GL.glBegin(GL.GL_TRIANGLE_STRIP);
            GL.glNormal3d(0, 0, 1);

            GL.glVertex3f(5.0f, 4.0f, 10.0f);//1
            GL.glVertex3f(0.0f, 4.0f, 13.0f);//2
            GL.glVertex3f(0.0f, 0.0f, 22.0f);//3
            GL.glEnd();


          //  GL.glRotatef(15, 1, 0, 0);

           

            GL.glPopMatrix();
        }

        public void drawNofar()
        {

            GL.glPushMatrix();
            GL.glRotated(sun, 0, 1, 0);

            GL.glTranslated(0, 245, 0);




            for (int j = 0; j < 8; j++) //leafs
            {
                drawNofarFlower();
                GL.glRotated(45, 0, 1, 0);
            }


            GL.glTranslated(0, -245, 0);

            GL.glTranslated(0, 100, 0);

            float[] flowerBase_ambuse = { 0.9117f, 0.2296f, 0.5562f, 0.0f };

            GL.glMaterialfv(GL.GL_FRONT_AND_BACK, GL.GL_AMBIENT_AND_DIFFUSE, flowerBase_ambuse);


            GL.glTranslated(0, 0 + (lake_size / 18), 0);

            float[] flowerHeart_ambuse = { 0.5117f, 0.0196f, 0.5562f, 0.0f };

            GL.glMaterialfv(GL.GL_FRONT_AND_BACK, GL.GL_AMBIENT_AND_DIFFUSE, flowerHeart_ambuse);

 
            GL.glRotated(sun, 0, 1, 0);

            GL.glRotatef(-90, 1, 0, 0);
            GLUT.glutSolidSphere(8 * lake_size / 200, 50, 50);


            GL.glTranslated(0, -0 - (lake_size / 8), 0);

            GL.glTranslated(0, -100, 0);
            GL.glPopMatrix();
        }

        public void drawNofarShade()
        {

            GL.glPushMatrix();
            GL.glRotated(sun, 0, 1, 0);

            GL.glTranslated(0, 245, 0);

            //   drawNofarFlower();




            for (int j = 0; j < 8; j++) //leafs
            {
                drawNofarFlower();
                GL.glRotated(45, 0, 1, 0);
            }


            GL.glTranslated(0, -245, 0);

            GL.glTranslated(0, 100, 0);

            float[] flowerBase_ambuse = { 0.0f, 0.0f, 0.0f, 0.0f };

            GL.glMaterialfv(GL.GL_FRONT_AND_BACK, GL.GL_AMBIENT_AND_DIFFUSE, flowerBase_ambuse);


            GL.glTranslated(0, 0 + (lake_size / 18), 0);

            float[] flowerHeart_ambuse = { 0.0f, 0.0f, 0.0f, 0.0f };

            GL.glMaterialfv(GL.GL_FRONT_AND_BACK, GL.GL_AMBIENT_AND_DIFFUSE, flowerHeart_ambuse);


            GL.glRotated(sun, 0, 1, 0);

            GL.glRotatef(-90, 1, 0, 0);
            //GLUT.glutSolidSphere(8 * lake_size / 200, 50, 50);


            GL.glTranslated(0, -0 - (lake_size / 8), 0);

            GL.glTranslated(0, -100, 0);
            GL.glPopMatrix();
        }


        public void drawSeaWead()
        {


            GL.glPushMatrix();

            GL.glRotatef(-90, 1.0f, 0.0f, 0.0f);



            //GL.glColor4d(0.75f, 0.55f, 0.15f, 1.0f);
            float[] seaWead_ambuse = { 0.7f, 0.5f, 0.1f, 1.0f };

            GL.glMaterialfv(GL.GL_FRONT_AND_BACK, GL.GL_AMBIENT_AND_DIFFUSE, seaWead_ambuse);


            float[] seaWead_ambuse2 = { 0.0f, 0.2f, 0.0f, 1.0f };

            GL.glMaterialfv(GL.GL_FRONT_AND_BACK, GL.GL_AMBIENT_AND_DIFFUSE, seaWead_ambuse2);

            ////
            ///
            GL.glColor3f(0.0f, 0.2f, 0.0f);
            GL.glBegin(GL.GL_TRIANGLE_STRIP);
            GL.glNormal3d(0, 0, 1);
            GL.glVertex3f(-6f, 0.0f, 0.0f);//1
            GL.glVertex3f(6f, 0.0f, 0.0f);//2

            GL.glVertex3f(2.0f, 1.0f + (float)-swing*0.2f, 10.0f);//3
            GL.glVertex3f(10.0f, 1.0f + (float)-swing * 0.2f, 10.0f);//4

            GL.glVertex3f(-2.0f, 0.0f + (float)swing * 0.2f, 18.0f);//5
            GL.glVertex3f(8.0f, 0.0f + (float)swing * 0.2f, 18.0f);//6

            GL.glVertex3f(3.0f, 1.0f + (float)-swing * 0.2f, 26.0f);//7
            GL.glVertex3f(13.0f, 1.0f + (float)-swing * 0.2f, 26);//8

            GL.glVertex3f(1.0f, -0.5f + (float)swing * 0.2f, 34.0f);//9

            GL.glVertex3f(5.0f, 0.0f + (float)swing * 0.2f, 37);//10

            float[] seaWead_ambuse3 = { 0.6f, 0.8f, 0.0f, 1.0f };

            GL.glMaterialfv(GL.GL_FRONT_AND_BACK, GL.GL_AMBIENT_AND_DIFFUSE, seaWead_ambuse3);

            GL.glVertex3f(-1.5f, 0.0f + (float)swing * 0.2f, 39);//10


            GL.glEnd();



            GL.glPopMatrix();

        }

        public void drawSeaWeadShade()
        {


            GL.glPushMatrix();

            GL.glRotatef(-90, 1.0f, 0.0f, 0.0f);



            GL.glColor4d(0.0f, 0.0f, 0.0f, 1.0f);
            float[] seaWead_ambuse = { 0.0f, 0.0f, 0.0f, 1.0f };

            GL.glMaterialfv(GL.GL_FRONT_AND_BACK, GL.GL_AMBIENT_AND_DIFFUSE, seaWead_ambuse);


            float[] seaWead_ambuse2 = { 0.0f, 0.0f, 0.0f, 1.0f };

            GL.glMaterialfv(GL.GL_FRONT_AND_BACK, GL.GL_AMBIENT_AND_DIFFUSE, seaWead_ambuse2);

            ////
            ///
            GL.glColor3f(0.0f, 0.0f, 0.0f);
            GL.glBegin(GL.GL_TRIANGLE_STRIP);
            GL.glNormal3d(0, 0, 1);
            GL.glVertex3f(-6f, 0.0f, 0.0f);//1
            GL.glVertex3f(6f, 0.0f, 0.0f);//2

            GL.glVertex3f(2.0f, 1.0f + (float)-swing * 0.2f, 10.0f);//3
            GL.glVertex3f(10.0f, 1.0f + (float)-swing * 0.2f, 10.0f);//4

            GL.glVertex3f(-2.0f, 0.0f + (float)swing * 0.2f, 18.0f);//5
            GL.glVertex3f(8.0f, 0.0f + (float)swing * 0.2f, 18.0f);//6

            GL.glVertex3f(3.0f, 1.0f + (float)-swing * 0.2f, 26.0f);//7
            GL.glVertex3f(13.0f, 1.0f + (float)-swing * 0.2f, 26);//8

            GL.glVertex3f(1.0f, -0.5f + (float)swing * 0.2f, 34.0f);//9

            GL.glVertex3f(5.0f, 0.0f + (float)swing * 0.2f, 37);//10

            float[] seaWead_ambuse3 = { 0.6f, 0.8f, 0.0f, 1.0f };

            GL.glMaterialfv(GL.GL_FRONT_AND_BACK, GL.GL_AMBIENT_AND_DIFFUSE, seaWead_ambuse3);

            GL.glVertex3f(-1.5f, 0.0f + (float)swing * 0.2f, 39);//10


            GL.glEnd();



            GL.glPopMatrix();

        }


        public void drawfish()
        {
            GL.glEnable(GL.GL_LIGHTING);
            GL.glPushMatrix();
            GL.glRotatef(-90, 1.0f, 0.0f, 0.0f);
            GL.glScalef(1.0f / 3.0f, 1.0f / 4.0f, 1.0f / 4.0f);
            // drawaxe();

            GL.glRotated((++speed) * speed_inc, 0.0, 0.0, 1.0);

            //drawaxe();
            GL.glTranslated(0.0, circle* circle_inc, 0);
            // drawaxe();
            GL.glRotated(90, 0.0, 0.0, 1.0);

            if (!swingFlg)
            {
                if (swing > -12)
                {
                    swing -= 0.4;
                }
                if (swing <= -12)
                    swingFlg = true;
            }
            if (swingFlg)
            {
                if (swing < 0)
                    swing += 0.4;

                if (swing >= 0)
                    swingFlg = false;
            }

            if (!checkBox)
            {

                if (!flapFlg)
                {
                    if (g < 1)
                    {
                        g += 0.2;
                        flap += 0.2f;
                    }
                    else
                    {
                        g += 0.2;
                        flap -= 0.2f;
                    }

                    if (g >= 2)
                    {
                        flapFlg = true;
                        g = 0;
                    }
                }

                if (flapFlg)
                {
                    if (g < 1)
                    {
                        g += 0.2;
                        flap -= 0.2f;
                    }
                    else
                    {
                        g += 0.2;
                        flap += 0.2f;
                    }
                    if (g >= 2)
                    {
                        flapFlg = false;
                        g = 0;
                    }
                }
            }
            if (checkBox)
                flap = 0;



            if (!heightFlg)
            {
                if (ascend < 50)
                {
                    ascend += 0.2;
                }
                if (ascend >= 25)
                    heightFlg = true;
            }
            if (heightFlg)
            {
                ascend -= 0.4;
                if (ascend <= 2)
                    heightFlg = false;
            }

            GL.glTranslated(0.0, -(circle), 0);

            if (!spirlFlg)
            {
                if (circle < 200)
                {
                    circle++;
                    if (circle <= 100)
                        updown += 0.2;
                    //          if(scale_inc<35)
                    //  scale_inc *=1.007;



                    if (circle > 100 && circle <= 155)
                        updown -= 0.2;

                    if (!heightFlg)
                    {
                        if (ascend < 15)
                        {
                            // ascend += 0.25;
                        }
                        if (ascend >= 15)
                            heightFlg = true;
                    }

                }
                if (circle == 200)
                    spirlFlg = true;
            }
            if (spirlFlg)
            {
                //if (scale_inc > 1)
                //    scale_inc *=0.992;

                circle--;

                if (circle >= 150)
                    updown -= 0.2;
                if (circle < 150 && circle > 100)
                    updown += 0.2;

                if (heightFlg)
                {
                    //  ascend -= 0.25;
                    if (ascend <= 2)
                        heightFlg = false;
                }

                if (circle <= 45)
                    spirlFlg = false;
            }


            if (!bublFlg)
            {
                if (bubble_up < 20)
                {
                    bub_Size+=0.05;
                    bubble_up += 0.75;
                    f += 0.1;
                }
                else
                {
                    bublFlg = true;
                    bub_Size =0;
                    bubble_up = 400;
                }
            }

            if (bublFlg)
            {
                bubble_up++;
                f -= 0.15;
                if (bubble_up >= 413)
                {
                    bubble_up = 0;
                    bublFlg = false;    
                }
            }



            //Up and down - Fish
            GL.glTranslatef(0.0f, 0.0f, 318.5f + (float)swing*13f);


            GL.glTranslatef(0.0f, 32.0f, 0.0f);
            // drawaxe();

            GL.glTranslatef(0.0f, -32.0f, 0.0f);


            GL.glScaled(scale_inc, scale_inc, scale_inc); //size of plane 
                                                         


            //Draw Bubble
            GL.glTranslated(0, 13 - bubble_up*0.1, 10 + bubble_up);

            float[] bub_ambuse = { 0.7f, 0.9f, 1f, 0.5f };
            GL.glMaterialfv(GL.GL_FRONT_AND_BACK, GL.GL_AMBIENT_AND_DIFFUSE, bub_ambuse);

            GLUT.glutSolidSphere(bub_Size*0.8, 32, 32);
            GL.glTranslated(0, -13 +bubble_up*0.1, -10 -bubble_up);




            //Draw eyes - White
            float[] eyes_ambuse = { 1f, 1f, 1f, 1.0f };
            GL.glMaterialfv(GL.GL_FRONT_AND_BACK, GL.GL_AMBIENT_AND_DIFFUSE, eyes_ambuse);

            GL.glTranslated(-1.5, 9.0, 12.5);
            GLUT.glutSolidSphere(1, 32, 32);
            GL.glTranslated(1.5, -9.0, -12.5);

            GL.glTranslated(1.5, 9.0, 12.5);
            GLUT.glutSolidSphere(1, 32, 32);
            GL.glTranslated(-1.5, -9.0, -12.5);

            //Draw eyes - Black
            float[] eyesBlack_ambuse = { 0f, 0f, 0f, 1.0f };
            GL.glMaterialfv(GL.GL_FRONT_AND_BACK, GL.GL_AMBIENT_AND_DIFFUSE, eyesBlack_ambuse);

            GL.glTranslated(-2, 9.65, 13);
            GLUT.glutSolidSphere(0.4, 32, 32);
            GL.glTranslated(2, -9.65, -13);

            GL.glTranslated(2, 9.65, 13);
            GLUT.glutSolidSphere(0.4, 32, 32);
            GL.glTranslated(-2, -9.65, -13);


            //Draw Tongue
            float[] tongue_ambuse = { 0.8421f, 0.1406f, 0.0942f, 1.0f };
            GL.glMaterialfv(GL.GL_FRONT_AND_BACK, GL.GL_AMBIENT_AND_DIFFUSE, tongue_ambuse);

            //// up and down - (inside mouth)
            GL.glBegin(GL.GL_TRIANGLE_STRIP);
            GL.glNormal3d(0, 0, 1);
            GL.glVertex3f(0.0f, 11.5f, 10.0f + (float)-swing * 0.1f);
            GL.glVertex3f(-2.8f, 9.5f, 10.5f);
            GL.glVertex3f(2.8f, 9.5f, 10.5f);
            GL.glEnd();

            GL.glBegin(GL.GL_TRIANGLE_STRIP);
            GL.glNormal3d(0, 0, 1);
            GL.glVertex3f(0.0f, 11.5f, 10.0f + (float)swing * 0.1f);
            GL.glVertex3f(-2.8f, 9.5f, 10.5f);
            GL.glVertex3f(2.8f, 9.5f, 10.5f);
            GL.glEnd();


            //Draw fish body
            float[] fish_ambuse = { 0.6421f, 0.6406f, 0.9642f, 1.0f };

            GL.glMaterialfv(GL.GL_FRONT_AND_BACK, GL.GL_AMBIENT_AND_DIFFUSE, fish_ambuse);

            //// up-left
            GL.glBegin(GL.GL_TRIANGLE_STRIP);
            GL.glNormal3d(0, 0, 1);
            GL.glVertex3f(0.0f, 12.0f, 10.0f + (float)-swing * 0.1f);
            GL.glVertex3f(-3.0f, 10.0f, 10.5f);
            GL.glVertex3f(0.0f, 10.0f, 14f);
            GL.glVertex3f(-4.0f, 8.0f, 11.0f);
            GL.glVertex3f(0.0f, 5.0f, 16.0f);
            GL.glVertex3f(-3.5f, 3.0f, 12.0f);
            GL.glVertex3f(0.0f, 0.0f, 12.5f);
            GL.glEnd();

            //// up-right
            GL.glBegin(GL.GL_TRIANGLE_STRIP);
            GL.glNormal3d(0, 0, 1);

            GL.glVertex3f(0.0f, 12.0f, 10.0f + (float)-swing * 0.1f);
            GL.glVertex3f(3.0f, 10.0f, 10.5f);
            GL.glVertex3f(0.0f, 10.0f, 14f);
            GL.glVertex3f(4.0f, 8.0f, 11.0f);
            GL.glVertex3f(0.0f, 5.0f, 16.0f);
            GL.glVertex3f(3.5f, 3.0f, 12.0f);
            GL.glVertex3f(0.0f, 0.0f, 12.5f);
            GL.glEnd();



            float[] fish_ambuse2 = { 0.5421f, 0.4406f, 0.0742f, 1.0f };

            GL.glMaterialfv(GL.GL_FRONT_AND_BACK, GL.GL_AMBIENT_AND_DIFFUSE, fish_ambuse2);


            //// bottom-left
            GL.glBegin(GL.GL_TRIANGLE_STRIP);
            GL.glNormal3d(0, 0, 1);

            GL.glVertex3f(0.0f, 12.0f, 10.0f + (float)swing*0.1f);
            GL.glVertex3f(-3.0f, 10.0f, 10.5f);
            GL.glVertex3f(0.0f, 9.0f, 5.0f);
            GL.glVertex3f(-4.0f, 8.0f, 11.0f);
            GL.glVertex3f(0.0f, 5.0f, 4.5f);
            GL.glVertex3f(-3.5f, 3.0f, 12.0f);
            GL.glVertex3f(0.0f, 0.0f, 12.5f);

            GL.glEnd();


            //// bottom-right
            GL.glBegin(GL.GL_TRIANGLE_STRIP);
            GL.glNormal3d(0, 0, 1);

            GL.glVertex3f(0.0f, 12.0f, 10.0f + (float)swing * 0.1f);
            GL.glVertex3f(3.0f, 10.0f, 10.5f);
            GL.glVertex3f(0.0f, 9.0f, 5.0f);
            GL.glVertex3f(4.0f, 8.0f, 11.0f);
            GL.glVertex3f(0.0f, 5.0f, 4.5f);
            GL.glVertex3f(3.5f, 3.0f, 12.0f);
            GL.glVertex3f(0.0f, 0.0f, 12.5f);
            GL.glEnd();





            float[] fish_ambuse3 = { 0.2421f, 0.3406f, 0.7742f, 1.0f };

            GL.glMaterialfv(GL.GL_FRONT_AND_BACK, GL.GL_AMBIENT_AND_DIFFUSE, fish_ambuse3);

            //// top-flipper-left
            GL.glBegin(GL.GL_TRIANGLE_STRIP);
            GL.glNormal3d(0, 0, 1);

            GL.glVertex3f(0.0f, 7.0f, 14.0f);
            GL.glVertex3f(-2.0f, 3.0f, 12.0f);
            GL.glVertex3f(0.0f, 2.5f, 18.5f);
            GL.glVertex3f(0.0f, 2.7f, 11.5f);

            GL.glEnd();

            float[] fish_ambuse4 = { 0.2421f, 0.2406f, 0.7742f, 1.0f };

            GL.glMaterialfv(GL.GL_FRONT_AND_BACK, GL.GL_AMBIENT_AND_DIFFUSE, fish_ambuse4);

            //// top-flipper-right
            GL.glBegin(GL.GL_TRIANGLE_STRIP);
            GL.glNormal3d(0, 0, 1);

            GL.glVertex3f(0.0f, 7.0f, 14.0f);
            GL.glVertex3f(2.0f, 3.0f, 12.0f);
            GL.glVertex3f(0.0f, 2.5f, 18.5f);
            GL.glVertex3f(0.0f, 2.7f, 11.5f);

            GL.glEnd();




            float[] fish_ambuse5 = { 0.7921f, 0.3906f, 0.0742f, 1.0f };

            GL.glMaterialfv(GL.GL_FRONT_AND_BACK, GL.GL_AMBIENT_AND_DIFFUSE, fish_ambuse3);

            //// bottom-flipper-left
            GL.glBegin(GL.GL_TRIANGLE_STRIP);
            GL.glNormal3d(0, 0, 1);


            GL.glVertex3f(0.0f, 5.5f, 5.0f);
            GL.glVertex3f(-0.5f, 4.0f, 6.5f);
            GL.glVertex3f(0.0f + (float)flap * 0.5f, 2.0f, 3.5f);
            GL.glVertex3f(0.0f, 3.0f, 9.0f);

            GL.glEnd();

            float[] fish_ambuse6 = { 0.6021f, 0.0406f, 0.0742f, 1.0f };

            GL.glMaterialfv(GL.GL_FRONT_AND_BACK, GL.GL_AMBIENT_AND_DIFFUSE, fish_ambuse4);

            //// bottom-flipper-right
            GL.glBegin(GL.GL_TRIANGLE_STRIP);
            GL.glNormal3d(0, 0, 1);

            GL.glVertex3f(0.0f, 5.5f, 5.0f);
            GL.glVertex3f(0.5f, 4.0f, 6.5f);
            GL.glVertex3f(0.0f + (float)flap*0.5f, 2.0f, 3.5f);
            GL.glVertex3f(0.0f, 3.0f, 9.0f);
            GL.glEnd();




            float[] fish_ambuse7 = { 0.6021f, 0.0406f, 0.0742f, 1.0f };
            GL.glMaterialfv(GL.GL_FRONT_AND_BACK, GL.GL_AMBIENT_AND_DIFFUSE, fish_ambuse4);

            ////top-tail-left
            GL.glBegin(GL.GL_TRIANGLE_STRIP);
            GL.glNormal3d(0, 0, 1);

            GL.glVertex3f(0.0f, 2.5f, 13.0f);
            GL.glVertex3f(-1.5f, 0.5f, 12.5f);
            GL.glVertex3f(0.0f + (float)flap*2, -2.0f, 19.0f);
            GL.glVertex3f(0.0f + (float)flap * 2, -1.0f, 12.5f);
            GL.glEnd();

            float[] fish_ambuse8 = { 0.6021f, 0.0406f, 0.0742f, 1.0f };
            GL.glMaterialfv(GL.GL_FRONT_AND_BACK, GL.GL_AMBIENT_AND_DIFFUSE, fish_ambuse4);

            //// top-tail-right
            GL.glBegin(GL.GL_TRIANGLE_STRIP);
            GL.glNormal3d(0, 0, 1);
            GL.glVertex3f(0.0f, 2.5f, 13.0f);
            GL.glVertex3f(1.5f, 0.5f, 12.5f);
            GL.glVertex3f(0.0f + (float)flap * 2, -2.0f, 19.0f);
            GL.glVertex3f(0.0f + (float)flap * 2, -1.0f, 12.5f);
            GL.glEnd();

            float[] fish_ambuse9 = { 0.4021f, 0.0206f, 0.0742f, 1.0f };
            GL.glMaterialfv(GL.GL_FRONT_AND_BACK, GL.GL_AMBIENT_AND_DIFFUSE, fish_ambuse9);

            ////bottom-tail-left
            GL.glBegin(GL.GL_TRIANGLE_STRIP);
            GL.glNormal3d(0, 0, 1);
            GL.glVertex3f(0.0f, 3.0f, 11.0f);
            GL.glVertex3f(-1.5f, 0.5f, 12.5f);
            GL.glVertex3f(0.0f + (float)flap * 2, -5.0f, 7.0f);
            GL.glVertex3f(0.0f + (float)flap * 2, -1.0f, 12.5f);
            GL.glEnd();

            float[] fish_ambuse10 = { 0.6021f, 0.0206f, 0.0242f, 1.0f };
            GL.glMaterialfv(GL.GL_FRONT_AND_BACK, GL.GL_AMBIENT_AND_DIFFUSE, fish_ambuse10);

            //// bottom-tail-right
            GL.glBegin(GL.GL_TRIANGLE_STRIP);
            GL.glNormal3d(0, 0, 1);
            GL.glVertex3f(0.0f, 3.0f, 11.0f);
            GL.glVertex3f(1.5f, 0.5f, 12.5f);
            GL.glVertex3f(0.0f + (float)flap * 2, -5.0f, 7.0f);
            GL.glVertex3f(0.0f + (float)flap * 2, -1.0f, 12.5f);
            GL.glEnd();





            float[] flippersU_ambuse = { 0.4921f, 0.0206f, 0.0642f, 1.0f };
            GL.glMaterialfv(GL.GL_FRONT_AND_BACK, GL.GL_AMBIENT_AND_DIFFUSE, flippersU_ambuse);

            //// left flipper up
            GL.glBegin(GL.GL_TRIANGLE_STRIP);
            GL.glNormal3d(0, 0, 1);

            GL.glVertex3f(-3.0f, 8.0f, 10.7f);
            GL.glVertex3f(-1.5f, 6.5f, 14.0f);
            GL.glVertex3f(-6.0f, 4.0f, 9.0f);
            GL.glVertex3f(-3.0f, 5.0f, 11.0f);
            GL.glEnd();

            //// right flipper up
            GL.glBegin(GL.GL_TRIANGLE_STRIP);
            GL.glNormal3d(0, 0, 1);

            GL.glVertex3f(3.0f, 8.0f, 10.7f);
            GL.glVertex3f(1.5f, 6.5f, 14.0f);
            GL.glVertex3f(6.0f, 4.0f, 9.0f);
            GL.glVertex3f(3.0f, 5.0f, 11.0f);
            GL.glEnd();



            float[] flippersD_ambuse = { 0.4928f, 0.0208f, 0.0842f, 1.0f };

            GL.glMaterialfv(GL.GL_FRONT_AND_BACK, GL.GL_AMBIENT_AND_DIFFUSE, flippersD_ambuse);

            //// left flipper down
            GL.glBegin(GL.GL_TRIANGLE_STRIP);
            GL.glNormal3d(0, 0, 1);
            GL.glVertex3f(-3.0f, 8.0f, 10.7f);
            GL.glVertex3f(-1.5f, 6.5f, 9.5f);
            GL.glVertex3f(-6.0f, 4.0f, 9.0f);
            GL.glVertex3f(-3.0f, 5.0f, 11.0f);
            GL.glEnd();

            //// right flipper dwon
            GL.glBegin(GL.GL_TRIANGLE_STRIP);
            GL.glNormal3d(0, 0, 1);
            GL.glVertex3f(3.0f, 8.0f, 10.7f);
            GL.glVertex3f(1.5f, 6.5f, 9.5f);
            GL.glVertex3f(6.0f, 4.0f, 9.0f);
            GL.glVertex3f(3.0f, 5.0f, 11.0f);
            GL.glEnd();



            GL.glBegin(GL.GL_TRIANGLE_STRIP);
            GL.glNormal3d(0, 0, 1);

            GL.glVertex3f(0.0f, 11.5f, 10.0f + (float)-swing * 0.1f);
            GL.glVertex3f(-2.8f, 9.5f, 10.5f);
            GL.glVertex3f(2.8f, 9.5f, 10.5f);
            GL.glEnd();

            GL.glBegin(GL.GL_TRIANGLE_STRIP);
            GL.glNormal3d(0, 0, 1);

            GL.glVertex3f(0.0f, 11.5f, 10.0f + (float)swing * 0.1f);
            GL.glVertex3f(-2.8f, 9.5f, 10.5f);
            GL.glVertex3f(2.8f, 9.5f, 10.5f);


            GL.glEnd();


            //// Black lines
            float[] lines_ambuse = { 0.4f, 0.4f, 0f, 1.0f };

            GL.glMaterialfv(GL.GL_FRONT_AND_BACK, GL.GL_AMBIENT_AND_DIFFUSE, lines_ambuse);

            //// up-left
            GL.glBegin(GL.GL_LINE_STRIP);
            GL.glVertex3f(0.0f, 12.0f, 10.0f + (float)-swing * 0.1f);
            GL.glVertex3f(-3.0f, 10.0f, 10.5f);
            GL.glVertex3f(0.0f, 10.0f, 14f);
            GL.glEnd();
            //// up-right

            GL.glBegin(GL.GL_LINE_STRIP);
            GL.glNormal3d(0, 0, 1);
            GL.glVertex3f(0.0f, 12.0f, 10.0f + (float)-swing * 0.1f);
            GL.glVertex3f(3.0f, 10.0f, 10.5f);
            GL.glVertex3f(0.0f, 10.0f, 14f);
            GL.glVertex3f(4.0f, 8.0f, 11.0f);
            GL.glEnd();

            GL.glPopMatrix();

        }

        public void drawfishShade()
        {

            GL.glPushMatrix();
            //  drawaxe();


            GL.glRotatef(-90, 1.0f, 0.0f, 0.0f);
            //   GL.glRotatef(45, 0.0f, 0.0f, 1.0f);
            // GL.glRotatef(25, 1.0f, 0.0f, 0.0f);
            GL.glScalef(1.0f / 3.0f, 1.0f / 4.0f, 1.0f / 4.0f);
            // drawaxe();

            GL.glRotated((++speed) * speed_inc, 0.0, 0.0, 1.0);

           // drawaxe();
            GL.glTranslated(0.0, circle * circle_inc, 0);
            // drawaxe();
            GL.glRotated(90, 0.0, 0.0, 1.0);


            if (!heightFlg)
            {
                if (ascend < 15)
                {
                    ascend += 0.25;
                }
                if (ascend >= 15)
                    heightFlg = true;
            }
            if (heightFlg)
            {
                ascend -= 0.25;
                if (ascend <= 2)
                    heightFlg = false;
            }


            GL.glRotated(hh, 0.0, 1.0, 0.0);

            //drawaxe();



            GL.glTranslated(0.0, -(circle), 0);
            GL.glTranslated(0.0, -(circle), 0);

            if (!spirlFlg)
            {
                if (circle < 200)
                {
                    circle++;
                    if (circle <= 100)
                        updown += 0.2;
                    if (circle > 100 && circle <= 155)
                        updown -= 0.2;

                    if (!heightFlg)
                    {
                        if (ascend < 15)
                        {
                            ascend += 0.25;
                        }
                        if (ascend >= 15)
                            heightFlg = true;
                    }

                }
                if (circle == 200)
                    spirlFlg = true;
            }
            if (spirlFlg)
            {

                //if (!swingFlg)
                //{
                //    if (hh > -28)
                //    {
                //        hh -= 2.5;
                //    }
                //    if (hh <= -28)
                //        swingFlg = true;
                //}
                //if (swingFlg)
                //{
                //    hh += 0.5;
                //    if (hh >= 0)
                //        swingFlg = false;
                //}

                circle--;

                if (circle >= 150)
                    updown -= 0.2;
                if (circle < 150 && circle > 100)
                    updown += 0.2;

                if (heightFlg)
                {
                    ascend -= 0.25;
                    if (ascend <= 2)
                        heightFlg = false;
                }

                if (circle <= 45)
                    spirlFlg = false;
            }




            //if (!flapFlg)
            //{
            //    f++;
            //    flap += 0.2f;
            //    if (f == 100)
            //        flapFlg = true;
            //}
            //if (flapFlg)
            //{
            //    f--;
            //    flap -= 0.2f;
            //    if (f == 0)
            //        flapFlg = false;
            //}


            GL.glTranslatef(0.0f, 0.0f, 18.5f);


            GL.glTranslatef(0.0f, 32.0f, 0.0f);
            // drawaxe();

            GL.glRotated(updown, 1.0, 0.0, 0.0);
            GL.glTranslatef(0.0f, -32.0f, 0.0f);


            GL.glPushMatrix();

            GL.glScaled(scale_inc, scale_inc, scale_inc); //size of
            //drawaxe();


            //Drawing the fish
            //// up-left
            GL.glColor3d(0, 0, 0);
            GL.glBegin(GL.GL_TRIANGLE_STRIP);
            GL.glNormal3d(0, 0, 1);

            GL.glVertex3f(0.0f, 12.0f, 10.0f);// + (float)flap + (float)ascend);//1   //hh= flap
            GL.glVertex3f(-3.0f, 10.0f, 10.5f);// + (float)flap + (float)ascend);//1   //hh= flap
            GL.glVertex3f(0.0f, 10.0f, 14f);// + ((float)flap * -0.15f) + (float)ascend);//2
            GL.glVertex3f(-4.0f, 8.0f, 11.0f);// + (float)flap + (float)ascend);//1   //hh= flap
            GL.glVertex3f(0.0f, 5.0f, 16.0f);
            GL.glVertex3f(-3.5f, 3.0f, 12.0f);
            GL.glVertex3f(0.0f, 0.0f, 12.5f);


            GL.glEnd();

            //// up-right
            GL.glBegin(GL.GL_TRIANGLE_STRIP);
            GL.glNormal3d(0, 0, 1);

            GL.glVertex3f(0.0f, 12.0f, 10.0f);
            GL.glVertex3f(3.0f, 10.0f, 10.5f);
            GL.glVertex3f(0.0f, 10.0f, 14f);
            GL.glVertex3f(4.0f, 8.0f, 11.0f);
            GL.glVertex3f(0.0f, 5.0f, 16.0f);
            GL.glVertex3f(3.5f, 3.0f, 12.0f);
            GL.glVertex3f(0.0f, 0.0f, 12.5f);


            GL.glEnd();







            //// bottom-left
            GL.glBegin(GL.GL_TRIANGLE_STRIP);
            GL.glNormal3d(0, 0, 1);

            GL.glVertex3f(0.0f, 12.0f, 10.0f);// + (float)flap + (float)ascend);//1   //hh= flap
            GL.glVertex3f(-3.0f, 10.0f, 10.5f);// + (float)flap + (float)ascend);//1   //hh= flap
            GL.glVertex3f(0.0f, 9.0f, 5.0f);// + ((float)flap * -0.15f) + (float)ascend);//2
            GL.glVertex3f(-4.0f, 8.0f, 11.0f);// + (float)flap + (float)ascend);//1   //hh= flap
            GL.glVertex3f(0.0f, 5.0f, 4.5f);
            GL.glVertex3f(-3.5f, 3.0f, 12.0f);
            GL.glVertex3f(0.0f, 0.0f, 12.5f);

            GL.glEnd();



            //// bottom-right
            GL.glBegin(GL.GL_TRIANGLE_STRIP);
            GL.glNormal3d(0, 0, 1);

            GL.glVertex3f(0.0f, 12.0f, 10.0f);// + (float)flap + (float)ascend);//1   //hh= flap
            GL.glVertex3f(3.0f, 10.0f, 10.5f);// + (float)flap + (float)ascend);//1   //hh= flap
            GL.glVertex3f(0.0f, 9.0f, 5.0f);// + ((float)flap * -0.15f) + (float)ascend);//2
            GL.glVertex3f(4.0f, 8.0f, 11.0f);// + (float)flap + (float)ascend);//1   //hh= flap
            GL.glVertex3f(0.0f, 5.0f, 4.5f);
            GL.glVertex3f(3.5f, 3.0f, 12.0f);
            GL.glVertex3f(0.0f, 0.0f, 12.5f);
            GL.glEnd();







            //// top-flipper-left
            GL.glBegin(GL.GL_TRIANGLE_STRIP);
            GL.glNormal3d(0, 0, 1);

            GL.glVertex3f(0.0f, 7.0f, 14.0f);
            GL.glVertex3f(-2.0f, 3.0f, 12.0f);
            GL.glVertex3f(0.0f, 2.5f, 18.5f);
            GL.glVertex3f(0.0f, 2.7f, 11.5f);

            GL.glEnd();


            //// top-flipper-right
            GL.glBegin(GL.GL_TRIANGLE_STRIP);
            GL.glNormal3d(0, 0, 1);

            GL.glVertex3f(0.0f, 7.0f, 14.0f);
            GL.glVertex3f(2.0f, 3.0f, 12.0f);
            GL.glVertex3f(0.0f, 2.5f, 18.5f);
            GL.glVertex3f(0.0f, 2.7f, 11.5f);


            GL.glEnd();






            //// bottom-flipper-left
            GL.glBegin(GL.GL_TRIANGLE_STRIP);
            GL.glNormal3d(0, 0, 1);


            GL.glVertex3f(0.0f, 5.5f, 5.0f);
            GL.glVertex3f(-0.5f, 4.0f, 6.5f);
            GL.glVertex3f(0.0f + (float)flap, 2.0f, 3.5f);
            GL.glVertex3f(0.0f, 3.0f, 9.0f);

            GL.glEnd();


            //// bottom-flipper-right
            GL.glBegin(GL.GL_TRIANGLE_STRIP);
            GL.glNormal3d(0, 0, 1);

            GL.glVertex3f(0.0f, 5.5f, 5.0f);
            GL.glVertex3f(0.5f, 4.0f, 6.5f);
            GL.glVertex3f(0.0f + (float)flap, 2.0f, 3.5f);
            GL.glVertex3f(0.0f, 3.0f, 9.0f);


            GL.glEnd();


            ////top-tail-left
            GL.glBegin(GL.GL_TRIANGLE_STRIP);
            GL.glNormal3d(0, 0, 1);

            GL.glVertex3f(0.0f, 2.5f, 13.0f);
            GL.glVertex3f(-1.5f, 0.5f, 12.5f);
            GL.glVertex3f(0.0f + (float)flap * 0.5f, -2.0f, 19.0f);
            GL.glVertex3f(0.0f + (float)flap * 0.5f, -1.0f, 12.5f);


            GL.glEnd();


            //// top-tail-right
            GL.glBegin(GL.GL_TRIANGLE_STRIP);
            GL.glNormal3d(0, 0, 1);

            GL.glVertex3f(0.0f, 2.5f, 13.0f);
            GL.glVertex3f(1.5f, 0.5f, 12.5f);
            GL.glVertex3f(0.0f + (float)flap * 0.5f, -2.0f, 19.0f);
            GL.glVertex3f(0.0f + (float)flap * 0.5f, -1.0f, 12.5f);


            GL.glEnd();

            ////bottom-tail-left
            GL.glBegin(GL.GL_TRIANGLE_STRIP);
            GL.glNormal3d(0, 0, 1);

            GL.glVertex3f(0.0f, 3.0f, 11.0f);
            GL.glVertex3f(-1.5f, 0.5f, 12.5f);
            GL.glVertex3f(0.0f + (float)flap * 0.5f, -5.0f, 7.0f);
            GL.glVertex3f(0.0f + (float)flap * 0.5f, -1.0f, 12.5f);


            GL.glEnd();

            //// bottom-tail-right
            GL.glBegin(GL.GL_TRIANGLE_STRIP);
            GL.glNormal3d(0, 0, 1);

            GL.glVertex3f(0.0f, 3.0f, 11.0f);
            GL.glVertex3f(1.5f, 0.5f, 12.5f);
            GL.glVertex3f(0.0f + (float)flap * 0.5f, -5.0f, 7.0f);
            GL.glVertex3f(0.0f + (float)flap * 0.5f, -1.0f, 12.5f);


            GL.glEnd();



            GL.glPopMatrix();
            GL.glPopMatrix();
        }

    }



}


