namespace RayTracer.Transformation

open RayTracer.Matrix

[<AutoOpen>]
module Domain =

    type Transformation =
        | Transform of x:float * y:float * z:float

module Transformation =

    let translation x y z : Matrix =
        Matrix.make [[]]


    let transformationMatrix (t:Transformation) : Matrix =
        match t with
        | Transform (x,y,z) ->
            Matrix.make [[1.; 0.; 0.; x];
                         [0.; 1.; 0.; y]
                         [0.; 0.; 1.; z]
                         [0.; 0.; 0.; 1.]]