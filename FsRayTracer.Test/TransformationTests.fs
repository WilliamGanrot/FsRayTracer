module TransformationTests

open System
open RayTracer.Point
open RayTracer.Vector
open RayTracer.Constnats
open RayTracer.Transformation
open RayTracer.Matrix

open Xunit


[<Fact>]
let ``multiplying a product by its inverse`` () =
    let transform = Transformation.translation 5. -3.0 2.
    let p = Point.create -3. 4. 5.

    let expected = Point.create 2. 1. 7.

    expected = Matrix.multiplyPoint transform p

[<Fact>]
let ``multiplying aby the inverse of a translation matrix`` () =

    //let m = Transform(5., -3., 2.) |> Transformation.transformationMatrix

    let transform = Transformation.translation 5. -3.0 2.


    let p = Point.create -3. 4. 5.
    
    let expected = Point.create -8. 7. 3.
    
    expected = Matrix.multiplyPoint transform p

[<Fact>]
let ``translation does not affect vetors`` () =
    let transform = Transformation.translation 5. -3.0 2.
    let v = Vector.create -3. 4. 5.
    

    (Matrix.multiplyVector transform v) = v |> Assert.True