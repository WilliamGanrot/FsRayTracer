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
    let transform = Translation(5., -3., 2.) |> Transformation.matrix
    let p = Point.create -3. 4. 5.

    let expected = Point.create 2. 1. 7.

    expected = Matrix.multiplyPoint transform p

[<Fact>]
let ``multiplying aby the inverse of a translation matrix`` () =

    let transform = Translation(5., -3., 2.) |> Transformation.matrix
    let p = Point.create -3. 4. 5.
    
    let expected = Point.create -8. 7. 3.
    
    expected = Matrix.multiplyPoint transform p

[<Fact>]
let ``translation does not affect vetors`` () =
    let transform = Translation(5., -3., 2.) |> Transformation.matrix
    let v = Vector.create -3. 4. 5.
    
    (Matrix.multiplyVector transform v) = v |> Assert.True

[<Fact>]
let ``a scaling matrix applied to point`` () =
    let transform = Scaling(2., 3., 4.) |> Transformation.matrix
    let p = Point.create -4. 6. 8.
    let expected = Point.create -8. 18. 32.
    
    (Matrix.multiplyPoint transform p) = expected |> Assert.True

[<Fact>]
let ``a scaling matrix applied to a vector`` () =
    let transform = Scaling(2., 3., 4.) |> Transformation.matrix
    let v = Vector.create -4. 6. 8.
    let expected = Vector.create -8. 18. 32.
    
    (Matrix.multiplyVector transform v) = expected |> Assert.True

[<Fact>]
let ``multiplying by the inverse of a scaling matrix`` () =
    let transform = Scaling(2., 3., 4.) |> Transformation.matrix
    let inv = Matrix.inverse transform
    let v = Vector.create -4. 6. 8.
    let expected = Vector.create -2. 2. 2.
    
    (Matrix.multiplyVector inv v) = expected |> Assert.True

[<Fact>]
let ``reflection is scaling by a negative value`` () =
    let transform = Scaling(-1., 1., 1.) |> Transformation.matrix
    let p = Point.create 2. 3. 4.
    let expected = Point.create -2. 3. 4.
    
    (Matrix.multiplyPoint transform p) = expected |> Assert.True

[<Fact>]
let ``roatating a point around the x axis`` () =
    let p = Point.create 0. 1. 0.
    let halfQ = Rotation(X, Math.PI/4.) |> Transformation.matrix
    let fullQ = Rotation(X, Math.PI/2.) |> Transformation.matrix

    let x = (Point.create 0. 0. 1.)
    let y = (Matrix.multiplyPoint fullQ p)

    Point.equal (Point.create 0. (Math.Pow(2., 0.5)/2.) (Math.Pow(2., 0.5)/2.)) (Matrix.multiplyPoint halfQ p) |> Assert.True
    Point.equal (Point.create 0. 0. 1.) (Matrix.multiplyPoint fullQ p) |> Assert.True

[<Fact>]
let ``the inverse of and x-rotation rotates in the opposite direction`` () =
    let p = Point.create 0. 1. 0.
    let halfQ = Rotation(X, Math.PI/4.) |> Transformation.matrix
    let inv = halfQ |> Matrix.inverse

    Point.equal (Matrix.multiplyPoint inv p) (Point.create 0. (Math.Pow(2., 0.5)) -(Math.Pow(2., 0.5)))

[<Fact>]
let ``roatating a point around the y axis`` () =
    let p = Point.create 0. 0. 1.
    let halfQ = Rotation(Y, Math.PI/4.) |> Transformation.matrix
    let fullQ = Rotation(Y, Math.PI/2.) |> Transformation.matrix

    Point.equal (Point.create (Math.Pow(2., 0.5)/2.) 0. (Math.Pow(2., 0.5)/2.)) (Matrix.multiplyPoint halfQ p) |> Assert.True
    Point.equal (Point.create 1. 0. 0.) (Matrix.multiplyPoint fullQ p) |> Assert.True

[<Fact>]
let ``roatating a point around the z axis`` () =
    let p = Point.create 0. 1. 0.
    let halfQ = Rotation(Z, Math.PI/4.) |> Transformation.matrix
    let fullQ = Rotation(Z, Math.PI/2.) |> Transformation.matrix

    Point.equal (Point.create -(Math.Pow(2., 0.5)/2.) (Math.Pow(2., 0.5)/2.) 0.) (Matrix.multiplyPoint halfQ p) |> Assert.True
    Point.equal (Point.create -1. 0. 0.) (Matrix.multiplyPoint fullQ p) |> Assert.True


