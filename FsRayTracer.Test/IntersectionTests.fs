module IntersectionTests


open System
open RayTracer.Point
open RayTracer.Vector
open RayTracer.Ray
open RayTracer.Matrix
open RayTracer.Intersection
open RayTracer.Transformation
open RayTracer.Helpers
open RayTracer.ObjectDomain
open RayTracer.Computation
open RayTracer.Triangle
open RayTracer.Sphere

open Xunit
open RayTracer.Object
open RayTracer.Constnats

[<Fact>]
let ``an intersection encapsulates t and object`` () =
    let s = Sphere.create()
    let i = Intersection.create s 3.5

    (FloatHelper.equal i.t 3.5) |> Assert.True
    s .=. i.object |> Assert.True

[<Fact>]
let ``aggregating intersections`` () =
    let s = Sphere.create()
    let i1 = Intersection.create s 1.
    let i2 = Intersection.create s 2.

    let xs = [i1; i2]

    xs.Length = 2 |> Assert.True
    xs.[0].t = 1. |> Assert.True
    xs.[1].t = 2. |> Assert.True

[<Fact>]
let ``intersect sets the object on the intersection`` () =
    let r = Ray.create (Point.create 0. 0. -5.) (Vector.create 0. 0. 1.)
    let s = Sphere.create()
    let xs = Ray.intersect s r

    xs.Length = 2 |> Assert.True
    xs.[0].object .=. s |> Assert.True
    xs.[1].object .=. s |> Assert.True

[<Fact>]
let ``the hit, when all intersections have positive t`` () =
    let s = Sphere.create()
    let i1 = Intersection.create s 1.
    let i2 = Intersection.create s 2.
    let xs = [i2; i1]

    let i = xs |> Intersection.hit
    match i with
    | Some v ->
        v .=. i1 |> Assert.True
    | None -> false |> Assert.True

[<Fact>]
let ``the hit, when some intersections have negative t`` () =
    let s = Sphere.create()
    let i1 = Intersection.create s -1.
    let i2 = Intersection.create s 1.
    let xs = [i2; i1]

    let i = xs |> Intersection.hit
    match i with
    | Some v ->
        v .=. i2 |> Assert.True
    | None -> false |> Assert.True

[<Fact>]
let ``the hit, when all intersections have negative t`` () =
    let s = Sphere.create()
    let i1 = Intersection.create s -2.
    let i2 = Intersection.create s -1.
    let xs = [i2; i1]

    let i = xs |> Intersection.hit
    match i with
    | None ->
        true |> Assert.True
    | _ -> false |> Assert.True


[<Fact>]
let ``the hit is always the lowest nonnegative intersection`` () =
    let s = Sphere.create()
    let i1 = Intersection.create s 5.
    let i2 = Intersection.create s 7.
    let i3 = Intersection.create s -3.
    let i4 = Intersection.create s 2.
    let xs = [i1; i2; i3; i4]

    let i = xs |> Intersection.hit
    match i with
    | Some v ->
        v .=. i4 |> Assert.True
    | None -> false |> Assert.True

[<Fact>]
let ``the under point is offset below the surface`` () =
    let r = Ray.create (Point.create 0. 0. -5.) (Vector.create 0. 0. 1.)
    let shape =
        Sphere.createGlass()
        |> Object.transform (Translation(0., 0., 1.))

    let i = Intersection.create shape 5.
    let comps = Computation.prepare r [i] [shape] i
    comps.underPoint.Z > epsilon/2. |> Assert.True
    comps.point.Z < comps.underPoint.Z |> Assert.True
    
[<Fact>]
let ``an intersection ac envausulate u and v`` () =
    let s = Triangle.create((Point.create 0. 1. 0.),(Point.create -1. 0. 0.),(Point.create 1. 0. 0.))
    let i = Intersection.intersectWithUV 3.5 0.2 0.4 s
    match i.uv with
    | Some (u,v) ->
        FloatHelper.equal 0.2 u |> Assert.True
        FloatHelper.equal 0.4 v |> Assert.True



