module IntersectionTests


open System
open RayTracer.Point
open RayTracer.Vector
open RayTracer.Ray
open RayTracer.Matrix
open RayTracer.Intersection
open RayTracer.Helpers

open Xunit
open RayTracer.Shape

[<Fact>]
let ``an intersection encapsulates t and object`` () =
    let s = Shape.sphere
    let i = Intersection.create s 3.5

    (FloatHelper.equal i.t 3.5) |> Assert.True
    s = i.object |> Assert.True

[<Fact>]
let ``aggregating intersections`` () =
    let s = Shape.sphere
    let i1 = Intersection.create s 1.
    let i2 = Intersection.create s 2.

    let xs = Intersection.intersections i1 i2

    xs.Length = 2 |> Assert.True
    xs.[0].t = 1. |> Assert.True
    xs.[1].t = 2. |> Assert.True

[<Fact>]
let ``intersect sets the object on the intersection`` () =
    let r = Ray.create (Point.create 0. 0. -5.) (Vector.create 0. 0. 1.)
    let s = Shape.sphere
    let xs = Ray.intersect s r

    xs.Length = 2 |> Assert.True
    xs.[0].object = s |> Assert.True
    xs.[1].object = s |> Assert.True

