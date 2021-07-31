module PointTests

open System
open RayTracer.Point
open RayTracer.Vector
open RayTracer.Constnats

open Xunit

[<Fact>]
let ``Can create point`` () =

    let made = createPoint (4.3, 1.0, 1.1)
    Assert.Equal(made.W, 1.0)
    0

[<Fact>]
let ``Subtracting a vector from a point`` () =
    let point = createPoint (3.0, 2.0, 1.0)
    let vector = createVector (5.0, 6.0, 7.0)

    let result = point - vector
    let expected = createPoint(-2.0, -4.0, -6.0)
    Assert.Equal(result, expected)
    0
