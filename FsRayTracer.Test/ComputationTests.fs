module ComputationTests

open System
open RayTracer.Point
open RayTracer.Vector
open RayTracer.Ray
open RayTracer.Matrix
open RayTracer.Intersection
open RayTracer.Helpers
open RayTracer.Computation
open RayTracer.Transformation

open Xunit
open RayTracer.Object

open RayTracer.Constnats

[<Fact>]
let ``precomputing the state of an intersection`` () =
    let r = Ray.create (Point.create 0. 0. -5.) (Vector.create 0. 0. 1.) 
    let s = Object.sphere()
    let i = Intersection.create s 4.
    let comps = i |> Computation.prepare r [i]

    FloatHelper.equal comps.t i.t |> Assert.True
    comps.object = i.object |> Assert.True
    Point.create 0. 0. -1. |> Point.equal comps.point |> Assert.True
    comps.eyev .= Vector.create 0. 0. -1. |> Assert.True
    comps.normalv .= Vector.create 0. 0. -1. |> Assert.True

[<Fact>]
let ``the hit, when an intersection occurs on the outside`` () =
    let r = Ray.create (Point.create 0. 0. -5.) (Vector.create 0. 0. 1.) 
    let s = Object.sphere()

    let comps =
        let i = Intersection.create s 4.
        Computation.prepare r [i] i

    comps.inside |> Assert.False

[<Fact>]
let ``the hit, when an intersection occurs on the inside`` () =
    let r = Ray.create (Point.create 0. 0. 0.) (Vector.create 0. 0. 1.) 
    let s = Object.sphere()
    let i = Intersection.create s 1.
    let comps = i |> Computation.prepare r [i]

    comps.point |> Point.equal (Point.create 0. 0. 1.) |> Assert.True
    comps.eyev .= (Vector.create 0. 0. -1.) |> Assert.True
    comps.normalv .= Vector.create 0. 0. -1. |> Assert.True
    comps.inside |> Assert.True

[<Fact>]
let ``the hit should offset the point`` () =
    let r = Ray.create (Point.create 0. 0. -5.) (Vector.create 0. 0. 1.) 
    let s =
        Object.sphere()
        |> Object.transform (Translation(0., 0., 0.1))

    let i = Intersection.create s 1.
    let comps = i |> Computation.prepare r [i]

    comps.overPoint.Z < -(epsilon/2.) |> Assert.True
    comps.point.Z > comps.overPoint.Z |> Assert.True

