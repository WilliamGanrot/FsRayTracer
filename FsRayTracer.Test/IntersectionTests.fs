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

    let xs = Intersection.intersections [i1; i2]

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

[<Fact>]
let ``the hit, when all intersections have positive t`` () =
    let s = Shape.sphere
    let i1 = Intersection.create s 1.
    let i2 = Intersection.create s 2.
    let xs = Intersection.intersections [i2; i1]

    let i = xs |> Intersection.hit
    i = Some (i1) |> Assert.True

[<Fact>]
let ``the hit, when some intersections have negative t`` () =
    let s = Shape.sphere
    let i1 = Intersection.create s -1.
    let i2 = Intersection.create s 1.
    let xs = Intersection.intersections [i2; i1]

    let i = xs |> Intersection.hit
    i = Some (i2) |> Assert.True

[<Fact>]
let ``the hit, when all intersections have negative t`` () =
    let s = Shape.sphere
    let i1 = Intersection.create s -2.
    let i2 = Intersection.create s -1.
    let xs = Intersection.intersections [i2; i1]

    let i = xs |> Intersection.hit
    i = None |> Assert.True


[<Fact>]
let ``the hit is always the lowest nonnegative intersection`` () =
    let s = Shape.sphere
    let i1 = Intersection.create s 5.
    let i2 = Intersection.create s 7.
    let i3 = Intersection.create s -3.
    let i4 = Intersection.create s 2.
    let xs = Intersection.intersections [i1; i2; i3; i4]

    let i = xs |> Intersection.hit
    i = Some(i4) |> Assert.True

