namespace RayTracer.Transformation

open RayTracer.Matrix
open System
open RayTracer.Point
open RayTracer.Vector
open RayTracer.ObjectDomain
open RayTracer.RayDomain


module Transformation =

    let rec matrix (t:Transformation) : Matrix =
        match t with
        | Matrix m -> m
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

    let applyToVector t v =
        t |> matrix |> Matrix.multiplyVector v

    let applyToMatrix t m =
        t |> matrix |> Matrix.multiply m

    let viewTransform (from:Point) (``to``:Point) (up:Vector) = 

        let forward = Vector.normalize (``to`` - from)
        let left =
            up
            |> Vector.normalize
            |> Vector.cross forward
        let trueUp = Vector.cross left forward

        let orientation =
            Matrix.make [ [left.X; left.Y; left.Z; 0.];
                          [trueUp.X; trueUp.Y; trueUp.Z; 0.];
                          [-forward.X; -forward.Y; -forward.Z; 0.];
                          [0.; 0.; 0.; 1.]]

        orientation
        |> applyToMatrix (Translation(-from.X, -from.Y, -from.Z))
        
        
        
        