module CameraTests

open System
open RayTracer.Point
open RayTracer.Vector
open RayTracer.Constnats
open RayTracer.Transformation
open RayTracer.Matrix
open RayTracer.Camera

open Xunit
open RayTracer.Ray
open RayTracer.Helpers


[<Fact>]
let ``constructing a camera`` () =
    let c = Camera.create 160 120 (Math.PI/2.)
    c.hsize = 160 |> Assert.True
    c.vsize = 120 |> Assert.True

    FloatHelper.equal c.fov (Math.PI/2.) |> Assert.True
    c.transform .= Matrix.identityMatrix 4 |> Assert.True

[<Fact>]
let ``the pixelsize for a horizontal canvas`` () =
    let c = Camera.create 200 125 (Math.PI/2.)
    c.pixelSize
    |> FloatHelper.equal 0.01
    |> Assert.True

[<Fact>]
let ``the pixelsize for a vertical canvas`` () =
    let c = Camera.create 125 200 (Math.PI/2.)
    c.pixelSize
    |> FloatHelper.equal 0.01
    |> Assert.True

[<Fact>]
let ``constructing a ray throgh the center of he canas`` () =
    let c = Camera.create 201 101 (Math.PI/2.)
    let r = Camera.rayForPixel 100. 50. c

    Point.equal r.origin (Point.create 0. 0. 0.) |> Assert.True
    r.direction .= (Vector.create 0. 0. -1.) |> Assert.True


    
[<Fact>]
let ``constructing a ray throgh a corer of the canvas`` () =
    let c = Camera.create 201 101 (Math.PI/2.)
    let r = Camera.rayForPixel 0. 0. c

    Point.equal r.origin (Point.create 0. 0. 0.) |> Assert.True
    r.direction .= (Vector.create 0.66519 0.33259 -0.66851) |> Assert.True

[<Fact>]
let ``constructing a ray when the camera is transformed`` () =
    let t =
        Transformation.matrix (Rotation(Y, (Math.PI/4.)))
        |> Transformation.applyToMatrix (Translation(0., -2., 5.))

    let c =
        Camera.create 201 101 (Math.PI/2.)
        |> Camera.withTransfom t

    let r = Camera.rayForPixel 100. 50. c

    Point.equal r.origin (Point.create 0. 2. -5.) |> Assert.True
    r.direction .= (Vector.create ((Math.Pow(2., 0.5))/2.) 0. -((Math.Pow(2., 0.5))/2.)) |> Assert.True
