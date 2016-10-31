//
//  main.m
//  PolyTri
//
//  Created by mac zdszkj on 2016/10/26.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//


#include <cstdlib>
#include <GLUT/glut.h>
#include <OpenGL/gl.h>
#include <time.h>
#include <fstream>
#include <string>
#include <sstream>
#include <algorithm>
#include <iterator>
#include <iostream>
using namespace std;

#include "../poly2tri/poly2tri.h"
using namespace p2t;

void Init();
//void ShutDown(int return_code);
void displayContent();
void Draw();
void DrawMap();
void ConstrainedColor(bool constrain);
double StringToDouble(const std::string& s);
double Random(double (*fun)(double), double xmin, double xmax);
double Fun(double x);

vector<Point *> CreateHole();

/// Dude hole examples
vector<Point*> CreateHeadHole();
vector<Point*> CreateChestHole();
float rotate_y = 0,
rotate_z = 0;
const float rotations_per_tick = .2;

/// Screen center x
double cx = 0.0;
/// Screen center y
double cy = 0.0;

/// Constrained triangles
vector<Triangle*> triangles;
/// Triangle map
list<Triangle*> map;
/// Polylines
vector< vector<Point*> > polylines;

/// Draw the entire triangle map?
bool draw_map = false;
/// Create a random distribution of points?
bool random_distribution = false;

template <class C> void FreeClear( C & cntr ) {
    for ( typename C::iterator it = cntr.begin();
         it != cntr.end(); ++it ) {
        delete * it;
    }
    cntr.clear();
}

int main()
{
    char *argv[5];
    int argc = 5;
    argv[0] = "p2t";
    argv[1] = "/Users/zdszkj_MacMini02/Desktop/ProjectTest/works/PolyTri/dude.txt";
    argv[2] = "300";
    argv[3] = "500";
    argv[4] = "2";
    glutInit(&argc,(char **)argv);
    glutInitDisplayMode(GLUT_SINGLE | GLUT_RGB);
    glutInitWindowSize(1000, 1000);
    glutInitWindowPosition(0, 0);
    glutCreateWindow("hello");

//    dude.dat 300 500 2
//    argc = 5;
//    argv[0] = "p2t";
//    argv[1] = "random";
//    argv[2] = "10";
//    argv[3] = "100";
//    argv[4] = "5.0";
   
    
    int num_points = 0;
    double max, min;
    double zoom;
    
    if (argc != 5) {
        cout << "-== USAGE ==-" << endl;
        cout << "Load Data File: p2t filename center_x center_y zoom" << endl;
        cout << "Example: ./build/p2t testbed/data/dude.dat 500 500 1" << endl;
        return 1;
    }
    
    if(string(argv[1]) == "random") {
        num_points = atoi(argv[2]);
        random_distribution = true;
        char* pEnd;
        max = strtod(argv[3], &pEnd);
        min = -max;
        cx = cy = 0;
        zoom = atof(argv[4]);
    } else {
        zoom = atof(argv[4]);
        cx = atof(argv[2]);
        cy = atof(argv[3]);
    }
    
    vector<p2t::Point*> polyline;
    
    if(random_distribution) {
        // Create a simple bounding box
        polyline.push_back(new Point(min,min));
        polyline.push_back(new Point(min,max));
        polyline.push_back(new Point(max,max));
        polyline.push_back(new Point(max,min));
    } else {
        // Load pointset from file
        
//        Point temP[4] = {{100,100},{100,400},{400,400},{400,100}};
//        for (int i = 0; i < 4; i++) {
//            Point p = temP[i];
//            float xValue = p.x;
//            float yValue = p.y;
//            polyline.push_back(new Point(xValue,yValue));
//            num_points++;
//        }
        
//         Parse and tokenize data file
        string line;
        ifstream myfile(argv[1]);
        if (myfile.is_open()) {
            while (!myfile.eof()) {
                getline(myfile, line);
                if (line.size() == 0) {
                    break;
                }
                istringstream iss(line);
                vector<string> tokens;
                copy(istream_iterator<string>(iss), istream_iterator<string>(),
                     back_inserter<vector<string> >(tokens));
                double x = StringToDouble(tokens[0]);
                double y = StringToDouble(tokens[1]);
                polyline.push_back(new Point(x, y));
                num_points++;
            }
            myfile.close();
        } else {
            cout << "File not opened" << endl;
        }
    }
    
    cout << "Number of constrained edges = " << polyline.size() << endl;
    polylines.push_back(polyline);
    
    Init();
    /*
     * Perform triangulation!
     */
    
//    double init_time = glfwGetTime();
    
    /*
     * STEP 1: Create CDT and add primary polyline
     * NOTE: polyline must be a simple polygon. The polyline's points
     * constitute constrained edges. No repeat points!!!
     */
    CDT* cdt = new CDT(polyline);
    
    /*
     * STEP 2: Add holes or Steiner points if necessary
     */
    
    string s(argv[1]);
     if (random_distribution) {
        max-=(1e-4);
        min+=(1e-4);
        for(int i = 0; i < num_points; i++) {
            double x = Random(Fun, min, max);
            double y = Random(Fun, min, max);
            cdt->AddPoint(new Point(x, y));
        }
     }else {
         
//         vector<Point *> hole = CreateHole();
//          num_points += hole.size();
//          cdt->AddHole(hole);
//         polylines.push_back(hole);
         
//          Add head hole
         vector<Point*> head_hole = CreateHeadHole();
         num_points += head_hole.size();
         cdt->AddHole(head_hole);
         // Add chest hole
         vector<Point*> chest_hole = CreateChestHole();
         num_points += chest_hole.size();
         cdt->AddHole(chest_hole);
         polylines.push_back(head_hole);
         polylines.push_back(chest_hole);
     }
    
    /*
     * STEP 3: Triangulate!
     */
    cdt->Triangulate();
    
//    double dt = glfwGetTime() - init_time;
    
    triangles = cdt->GetTriangles();
    map = cdt->GetMap();
    
    cout << "Number of points = " << num_points << endl;
    cout << "Number of triangles = " << triangles.size() << endl;
//    cout << "Elapsed time (ms) = " << dt*1000.0 << endl;
    
//    MainLoop(zoom);
    glutDisplayFunc(displayContent);
    
    glutMainLoop();
    // Cleanup
    
//    delete cdt;
//    
//    // Free points
//    for(int i = 0; i < polylines.size(); i++) {
//        vector<Point*> poly = polylines[i];
//        FreeClear(poly);
//    }
    return 0;
}

void Init()
{
    glClearColor(0.0,0.0,0.0,1.0);
    
    glMatrixMode(GL_PROJECTION);
    glLoadIdentity();
    glOrtho(0.0, 1.0, 0.0, 1.0, -1.0, 1.0);
}

void displayContent(){
    
    if (draw_map) {
        DrawMap();
    } else {
        Draw();
    }
    
    
    
}

void ResetZoom(double zoom, double cx, double cy, double width, double height)
{
    double left = -width / zoom;
    double right = width / zoom;
    double bottom = -height / zoom;
    double top = height / zoom;
    
    // Reset viewport
    glLoadIdentity();
    glMatrixMode(GL_PROJECTION);
    glLoadIdentity();
    
    // Reset ortho view
    glOrtho(left, right, bottom, top, 1, -1);
    glTranslatef(-cx, -cy, 0);
    glMatrixMode(GL_MODELVIEW);
    glDisable(GL_DEPTH_TEST);
    glLoadIdentity();
    
    // Clear the screen
    glClear(GL_COLOR_BUFFER_BIT);
}

void Draw()
{
    // reset zoom
    Point center = Point(cx, cy);

    ResetZoom(3.0f, center.x, center.y, 800, 600);
    
    for (int i = 0; i < triangles.size(); i++) {
        Triangle& t = *triangles[i];
        Point& a = *t.GetPoint(0);
        Point& b = *t.GetPoint(1);
        Point& c = *t.GetPoint(2);
        
        
        // Red
        glColor3f(1, 0, 0);
        glBegin(GL_LINE_LOOP);
        glVertex2f(a.x, a.y);
        glVertex2f(b.x, b.y);
        glVertex2f(c.x, c.y);
        printf("point:a.x = %f,a.y = %f,b.x = %f,b.y = %f,c.x = %f,c.y = %f\n",a.x,a.y,b.x,b.y,c.x,c.y);
        glEnd();
    }
    // green
    glColor3f(0, 1, 0);
    
    for(int i = 0; i < polylines.size(); i++) {
        vector<Point*> poly = polylines[i];
        glBegin(GL_LINE_LOOP);
        for(int j = 0; j < poly.size(); j++) {
            glVertex2f(poly[j]->x, poly[j]->y);
        }
        glEnd();
    }
    glFlush();
}

void DrawMap()
{
    // reset zoom
//    Point center = Point(cx, cy);
    
//    ResetZoom(zoom, center.x, center.y, 800, 600);
    
    list<Triangle*>::iterator it;
    for (it = map.begin(); it != map.end(); it++) {
        Triangle& t = **it;
        Point& a = *t.GetPoint(0);
        Point& b = *t.GetPoint(1);
        Point& c = *t.GetPoint(2);
        
        ConstrainedColor(t.constrained_edge[2]);
        glBegin(GL_LINES);
        glVertex2f(a.x, a.y);
        glVertex2f(b.x, b.y);
        glEnd( );
        
        ConstrainedColor(t.constrained_edge[0]);
        glBegin(GL_LINES);
        glVertex2f(b.x, b.y);
        glVertex2f(c.x, c.y);
        glEnd( );
        
        ConstrainedColor(t.constrained_edge[1]);
        glBegin(GL_LINES);
        glVertex2f(c.x, c.y);
        glVertex2f(a.x, a.y);
        glEnd( );
    }
    glFlush();
}

void ConstrainedColor(bool constrain)
{
    if (constrain) {
        // Green
        glColor3f(0, 1, 0);
    } else {
        // Red
        glColor3f(1, 0, 0);
    }
}

vector<Point*> CreateHeadHole() {
    
    vector<Point*> head_hole;
    head_hole.push_back(new Point(325, 437));
    head_hole.push_back(new Point(320, 423));
    head_hole.push_back(new Point(329, 413));
    head_hole.push_back(new Point(332, 423));
    
    return head_hole;
}

vector<Point*> CreateChestHole() {
    
    vector<Point*> chest_hole;
    chest_hole.push_back(new Point(320.72342,480));
    chest_hole.push_back(new Point(338.90617,465.96863));
    chest_hole.push_back(new Point(347.99754,480.61584));
    chest_hole.push_back(new Point(329.8148,510.41534));
    chest_hole.push_back(new Point(339.91632,480.11077));
    chest_hole.push_back(new Point(334.86556,478.09046));
    
    return chest_hole;
    
}

vector<Point*> CreateHole(){
    vector<Point *>hole;
    hole.push_back(new Point(200,300));
    hole.push_back(new Point(300,300));
    hole.push_back(new Point(300,250));
    hole.push_back(new Point(275,225));
    hole.push_back(new Point(250,200));
    hole.push_back(new Point(225,225));
    hole.push_back(new Point(200,250));
    return hole;
}


double StringToDouble(const std::string& s)
{
    std::istringstream i(s);
    double x;
    if (!(i >> x))
        return 0;
    return x;
}

double Fun(double x)
{
    return 2.5 + sin(10 * x) / x;
}

double Random(double (*fun)(double), double xmin = 0, double xmax = 1)
{
    static double (*Fun)(double) = NULL, YMin, YMax;
    static bool First = true;
    
    // Initialises random generator for first call
    if (First)
    {
        First = false;
        srand((unsigned) time(NULL));
    }
    
    // Evaluates maximum of function
    if (fun != Fun)
    {
        Fun = fun;
        YMin = 0, YMax = Fun(xmin);
        for (int iX = 1; iX < 100; iX++)
        {
            double X = xmin + (xmax - xmin) * iX / RAND_MAX;
            double Y = Fun(X);
            YMax = Y > YMax ? Y : YMax;
        }
    }
    
    // Gets random values for X & Y
    double X = xmin + (xmax - xmin) * rand() / RAND_MAX;
    double Y = YMin + (YMax - YMin) * rand() / RAND_MAX;
    
    // Returns if valid and try again if not valid
    return Y < fun(X) ? X : Random(Fun, xmin, xmax);
}

//二维和三维坐标空间点的转换

//三维空间中的同一个面上的点
//    JCVector3 point1 = JCVector3Make(3, 0, 3);
//    JCVector3 point2 = JCVector3Make(5, 0, 3);
//    JCVector3 point3 = JCVector3Make(5, 3, 3);
//    JCVector3 point4 = JCVector3Make(3, 3, 3);

//    JCVector3 point1 = JCVector3Make(3, 0, 3);
//    JCVector3 point2 = JCVector3Make(3, 0, 5);
//    JCVector3 point3 = JCVector3Make(3, 3, 5);
//    JCVector3 point4 = JCVector3Make(3, 3, 3);

//JCVector3 point1 = JCVector3Make(3, 0, 5);
//JCVector3 point2 = JCVector3Make(5, 0, 3);
//JCVector3 point3 = JCVector3Make(5, 3, 3);
//JCVector3 point4 = JCVector3Make(3, 3, 5);
//
//JCVector3 vectorXZ = JCVector3Subv(&point2,&point1);
//JCVector3 normalXZ = JCVector3Normalize(&vectorXZ);
//JCVector3 vectorXY = JCVector3Subv(&point4,&point1);
//JCVector3 normalXY = JCVector3Normalize(&vectorXY);
//
////三维空间中的同一个面上的点以左下角为原点的平面坐标系的表示
//float wallWidth = JCVector3Distance(&point2,&point1);
//float wallHeight = JCVector3Distance(&point4,&point1);
//JCVector2 point2D[4] = {{0,0},{static_cast<JCFloat>(wallWidth),0},{static_cast<JCFloat>(wallWidth),static_cast<JCFloat>(wallHeight)},{0,static_cast<JCFloat>(wallHeight)}};
//
//
////将这些点转换到三维空间中对应的点
//for (int i = 0; i < 4; i++) {
//    JCVector3 maX = JCVector3Muls(&normalXZ, point2D[i].x);
//    JCVector3 maY = JCVector3Muls(&normalXY, point2D[i].y);
//    JCVector3 result = JCVector3Make(point1.x + maX.x + maY.x, point1.y + maX.y + maY.y, point1.z + maX.z + maY.z );
//    //        printf("x = %f, y = %f,z = %f\n",result.x, result.y, result.z);
//}
