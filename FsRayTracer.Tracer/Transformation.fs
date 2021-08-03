namespace RayTracer.Transformation

open RayTracer.Matrix
open System

[<AutoOpen>]
module Domain =
    type Axis =
        | X
        | Y
        | Z

    type Skew =
        | Xy
        | Xz
        | Yx
        | Yz
        | Zx
        | Zy

    type Transformation =
        | Translation of x:float * y:float * z:float
        | Scaling of x:float * y:float * z:float
        | Rotation of axis:Axis * degrees:float
        | Shering of xy:float * xz:float * yx:float * yz:float * zx:float * zy:float
        | TransformationGroup of transformations:Transformation list
        
     

module Transformation =

    let rec matrix (t:Transformation) : Matrix =
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
        | Shering (xy,xz,yx,yz,zx,zy) ->
            Matrix.make [[1.;  xy;    xz;     0.];
                         [yx;  1.;    yz;     0.]
                         [zx;  zy;    1.;     0.]
                         [0.;  0.;    0.;     1.]]
        | TransformationGroup l ->
            match l with
            | h::[] -> h |> matrix
            | h::t ->
                t
                |> List.map(fun x -> x |> matrix)
                |> List.fold (*) (h |> matrix)
            | _ -> failwith "incomplete pattern matching"

    
    let applyToPoint t p =
        t |> matrix |> Matrix.multiplyPoint p
