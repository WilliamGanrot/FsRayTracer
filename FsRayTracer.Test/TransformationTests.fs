module TransformationTests

open System
open RayTracer.Point
open RayTracer.Vector
open RayTracer.Constnats
open RayTracer.Transformation
open RayTracer.Matrix
open RayTracer.ObjectDomain

open Xunit
open RayTracer.Ray


[<Fact>]
let ``multiplying a product by its inverse`` () =
    let transform = Translation(5., -3., 2.) |> Transformation.matrix
    let p = Point.create -3. 4. 5.

    let expected = Point.create 2. 1. 7.

    expected = Matrix.multiplyPoint p transform

[<Fact>]
let ``multiplying aby the inverse of a translation matrix`` () =

    let transform = Translation(5., -3., 2.) |> Transformation.matrix
    let p = Point.create -3. 4. 5.
    
    let expected = Point.create -8. 7. 3.
    
    expected = Matrix.multiplyPoint p transform

[<Fact>]
let ``translation does not affect vetors`` () =
    let transform = Translation(5., -3., 2.) |> Transformation.matrix
    let v = Vector.create -3. 4. 5.
    
    (Matrix.multiplyVector v transform) = v |> Assert.True

[<Fact>]
let ``a scaling matrix applied to point`` () =
    let transform = Scaling(2., 3., 4.) |> Transformation.matrix
    let p = Point.create -4. 6. 8.
    let expected = Point.create -8. 18. 32.
    
    (Matrix.multiplyPoint p transform) = expected |> Assert.True

[<Fact>]
let ``a scaling matrix applied to a vector`` () =
    let transform = Scaling(2., 3., 4.) |> Transformation.matrix
    let v = Vector.create -4. 6. 8.
    let expected = Vector.create -8. 18. 32.
    
    (Matrix.multiplyVector v transform) = expected |> Assert.True

[<Fact>]
let ``multiplying by the inverse of a scaling matrix`` () =
    let transform = Scaling(2., 3., 4.) |> Transformation.matrix
    let inv = Matrix.inverse transform
    let v = Vector.create -4. 6. 8.
    let expected = Vector.create -2. 2. 2.
    
    (Matrix.multiplyVector v inv) = expected |> Assert.True

[<Fact>]
let ``reflection is scaling by a negative value`` () =
    let transform = Scaling(-1., 1., 1.) |> Transformation.matrix
    let p = Point.create 2. 3. 4.
    let expected = Point.create -2. 3. 4.
    
    (Matrix.multiplyPoint p transform) = expected |> Assert.True

[<Fact>]
let ``roatating a point around the x axis`` () =
    let p = Point.create 0. 1. 0.
    let halfQ = Rotation(X, Math.PI/4.) |> Transformation.matrix
    let fullQ = Rotation(X, Math.PI/2.) |> Transformation.matrix

    let x = (Point.create 0. 0. 1.)
    let y = (Matrix.multiplyPoint p fullQ)

    Point.equal (Point.create 0. (Math.Pow(2., 0.5)/2.) (Math.Pow(2., 0.5)/2.)) (Matrix.multiplyPoint p halfQ) |> Assert.True
    Point.equal (Point.create 0. 0. 1.) (Matrix.multiplyPoint p fullQ) |> Assert.True

[<Fact>]
let ``the inverse of and x-rotation rotates in the opposite direction`` () =
    let p = Point.create 0. 1. 0.
    let halfQ = Rotation(X, Math.PI/4.) |> Transformation.matrix
    let inv = halfQ |> Matrix.inverse

    Point.equal (Matrix.multiplyPoint p inv) (Point.create 0. (Math.Pow(2., 0.5)) -(Math.Pow(2., 0.5)))

[<Fact>]
let ``roatating a point around the y axis`` () =
    let p = Point.create 0. 0. 1.
    let halfQ = Rotation(Y, Math.PI/4.) |> Transformation.matrix
    let fullQ = Rotation(Y, Math.PI/2.) |> Transformation.matrix

    Point.equal (Point.create (Math.Pow(2., 0.5)/2.) 0. (Math.Pow(2., 0.5)/2.)) (Matrix.multiplyPoint p halfQ) |> Assert.True
    Point.equal (Point.create 1. 0. 0.) (Matrix.multiplyPoint p fullQ) |> Assert.True

[<Fact>]
let ``roatating a point around the z axis`` () =
    let p = Point.create 0. 1. 0.
    let halfQ = Rotation(Z, Math.PI/4.) |> Transformation.matrix
    let fullQ = Rotation(Z, Math.PI/2.) |> Transformation.matrix

    Point.equal (Point.create -(Math.Pow(2., 0.5)/2.) (Math.Pow(2., 0.5)/2.) 0.) (Matrix.multiplyPoint p halfQ) |> Assert.True
    Point.equal (Point.create -1. 0. 0.) (Matrix.multiplyPoint p fullQ) |> Assert.True


[<Fact>]
let ``a shearring tranformation moves x in proportion to y`` () =
    let p = Point.create 2. 3. 4.
    let t = Shering(1., 0., 0., 0., 0., 0.) |> Transformation.matrix
    let expected = Point.create 5. 3. 4.

    Point.equal expected (Matrix.multiplyPoint p t) |> Assert.True

[<Fact>]
let ``a shearring tranformation moves x in proportion to z`` () =
    let p = Point.create 2. 3. 4.
    let t = Shering(0., 1., 0., 0., 0., 0.) |> Transformation.matrix
    let expected = Point.create 6. 3. 4.

    Point.equal expected (Matrix.multiplyPoint p t) |> Assert.True

[<Fact>]
let ``a shearring tranformation moves y in proportion to x`` () =
    let p = Point.create 2. 3. 4.
    let t = Shering(0., 0., 1., 0., 0., 0.) |> Transformation.matrix
    let expected = Point.create 2. 5. 4.

    Point.equal expected (Matrix.multiplyPoint p t) |> Assert.True

[<Fact>]
let ``a shearring tranformation moves y in proportion to z`` () =
    let p = Point.create 2. 3. 4.
    let t = Shering(0., 0., 0., 1., 0., 0.) |> Transformation.matrix
    let expected = Point.create 2. 7. 4.

    Point.equal expected (Matrix.multiplyPoint p t) |> Assert.True

[<Fact>]
let ``a shearring tranformation moves z in proportion to x`` () =
    let p = Point.create 2. 3. 4.
    let t = Shering(0., 0., 0., 0., 1., 0.) |> Transformation.matrix
    let expected = Point.create 2. 3. 6.

    Point.equal expected (Matrix.multiplyPoint p t) |> Assert.True

[<Fact>]
let ``a shearring tranformation moves z in proportion to y`` () =
    let p = Point.create 2. 3. 4.
    let t = Shering(0., 0., 0., 0., 0., 1.) |> Transformation.matrix
    let expected = Point.create 2. 3. 7.

    Point.equal expected (Matrix.multiplyPoint p t) |> Assert.True

[<Fact>]
let ``individual transformations are applied in sequence`` () =
    let p = Point.create 1. 0. 1.
    let a = Rotation(X, Math.PI/2.) |> Transformation.matrix
    let b = Scaling(5., 5., 5.) |> Transformation.matrix
    let c = Translation(10., 5., 7.) |> Transformation.matrix

    let p2 = Matrix.multiplyPoint p a
    Point.equal p2 (Point.create 1. -1. 0.) |> Assert.True

    let p3 = Matrix.multiplyPoint p2 b
    Point.equal p3 (Point.create 5. -5. 0.) |> Assert.True

    let p4 = Matrix.multiplyPoint p3 c
    Point.equal p4 (Point.create 15. 0. 7.) |> Assert.True

[<Fact>]
let ``chained transformations must be applied in reverse order`` () =
    let p = Point.create 1. 0. 1.
    let a = Rotation(X, Math.PI/2.)
    let b = Scaling(5., 5., 5.)
    let c = Translation(10., 5., 7.)

    let x = TransformationGroup [c;b;a] |> Transformation.matrix

    Matrix.multiplyPoint p x
    |> Point.equal (Point.create 15. 0. 7.)
    |> Assert.True

[<Fact>]
let ``a transformation matrix for the defaul orientation`` () =
    let from = Point.create 0. 0. 0.
    let ``to`` = Point.create 0. 0. -1.
    let up = Vector.create 0. 1. 0.

    let t = Transformation.viewTransform from ``to`` up
    t .= Matrix.identityMatrix 4

[<Fact>]
let ``a view transformation matrix looking in poisitive z directoin`` () =
    let from = Point.create 0. 0. 0.
    let ``to`` = Point.create 0. 0. 1.
    let up = Vector.create 0. 1. 0.


    let t = Transformation.viewTransform from ``to`` up
    t .= Transformation.matrix (Scaling(-1., 1., -1.))

[<Fact>]
let ``the view transformation moves the world`` () =
    let from = Point.create 0. 0. 8.
    let ``to`` = Point.create 0. 0. 0.
    let up = Vector.create 0. 1. 0.

    let t = Transformation.viewTransform from ``to`` up
    t .= Transformation.matrix (Translation(0., 0., -8.))

[<Fact>]
let ``an arbitrary view transformation`` () =
    let from = Point.create 1. 3. 2.
    let ``to`` = Point.create 4. -2. 8.
    let up = Vector.create 1. 1. 0.


    let t = Transformation.viewTransform from ``to`` up
    let expected =
        Matrix.make [ [ -0.50709; 0.50709; 0.67612; -2.36643 ];
                      [ 0.76772; 0.60609; 0.12122; -2.82843 ];
                      [ -0.35857; 0.59761; -0.71714; 0. ];
                      [ 0.; 0.; 0.; 1. ] ]


    t .= expected |> Assert.True
