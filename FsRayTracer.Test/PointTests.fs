module PointTests

open System
open RayTracer.Point
open RayTracer.Vector
open RayTracer.Constnats

open Xunit

[<Fact>]
let ``Can create point`` () =

    let made = Point.create 4.3 1. 1.
    Assert.Equal(made.W, 1.0)
    0

[<Fact>]
let ``Subtracting a vector from a point`` () =
    let point = Point.create 3. 2. 1.
    let vector = Vector.create 5.0 6.0 7.0

    let result = point - vector
    let expected = Point.create -2.0 -4.0 -6.0
    Assert.Equal(result, expected)
    0
