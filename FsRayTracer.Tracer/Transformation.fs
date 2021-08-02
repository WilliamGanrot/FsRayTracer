namespace RayTracer.Transformation

open RayTracer.Matrix
open System

[<AutoOpen>]
module Domain =
    type Axis =
        | X
        | Y
        | Z

    type Transformation =
        | Translation of x:float * y:float * z:float
        | Scaling of x:float * y:float * z:float
        | Rotation of axis:Axis * degrees:float

module Transformation =

    let translation x y z : Matrix =
        Matrix.make [[]]


    let matrix (t:Transformation) : Matrix =
        match t with
        | Translation (x,y,z) ->
            Matrix.make [[1.; 0.; 0.; x];
                         [0.; 1.; 0.; y]
                         [0.; 0.; 1.; z]
                         [0.; 0.; 0.; 1.]]
        | Scaling (x,y,z) ->
            Matrix.make [[x; 0.; 0.; 0.];
                         [0.; y; 0.; 0.]
                         [0.; 0.; z; 0.]
                         [0.; 0.; 0.; 1.]]
        | Rotation (X, angle) ->
            Matrix.make [[1.;   0.;                 0.;                 0.];
                         [0.;   Math.Cos(angle);    -Math.Sin(angle);   0.]
                         [0.;   Math.Sin(angle);    Math.Cos(angle);    0.]
                         [0.;   0.;                 0.;                 1.]]
        | Rotation (Y, angle) ->
            Matrix.make [[Math.Cos(angle);  0.;    Math.Sin(angle);     0.];
                         [0.;               1.;    0.;                  0.]
                         [-Math.Sin(angle); 0.;    Math.Cos(angle);     0.]
                         [0.;               0.;    0.;                  1.]]
        | Rotation (Z, angle) ->
            Matrix.make [[Math.Cos(angle);  -Math.Sin(angle);    0.;     0.];
                         [Math.Sin(angle);  Math.Cos(angle);    0.;     0.]
                         [0.;               0.;    1.;                  0.]
                         [0.;               0.;    0.;                  1.]]    
            