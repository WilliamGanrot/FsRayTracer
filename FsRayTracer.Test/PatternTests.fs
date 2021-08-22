module PatternTests

open System
open RayTracer.Point
open RayTracer.Vector
open RayTracer.Ray
open RayTracer.Matrix
open RayTracer.Intersection
open RayTracer.Helpers
open RayTracer.Transformation
open RayTracer.Color
open RayTracer.Pattern

open Xunit
open RayTracer.Object

open RayTracer.Constnats
open RayTracer.Material
open RayTracer.Light

[<Fact>]
let ```a stripe pattern is constant i y``() =
    let p = Pattern.stripes Color.white Color.black
    (p |> Pattern.at (Point.create 0. 0. 0.)) .= Color.white |> Assert.True
    (p |> Pattern.at (Point.create 0. 1. 0.)) .= Color.white |> Assert.True
    (p |> Pattern.at (Point.create 0. 2. 0.)) .= Color.white |> Assert.True

[<Fact>]
let ```a stripe pattern is constant i z``() =
    let p = Pattern.stripes Color.white Color.black
    (p |> Pattern.at (Point.create 0. 0. 0.)) .= Color.white |> Assert.True
    (p |> Pattern.at (Point.create 0. 0. 1.)) .= Color.white |> Assert.True
    (p |> Pattern.at (Point.create 0. 0. 1.)) .= Color.white |> Assert.True

[<Fact>]
let ```a stripe pattern is constant i x``() =
    let p = Pattern.stripes Color.white Color.black
    (p |> Pattern.at (Point.create 0. 0. 0.)) .= Color.white |> Assert.True
    (p |> Pattern.at (Point.create 0.9 0. 0.)) .= Color.white |> Assert.True
    (p |> Pattern.at (Point.create 1. 0. 0.)) .= Color.black |> Assert.True
    (p |> Pattern.at (Point.create -0.1 0. 0.)) .= Color.black |> Assert.True
    (p |> Pattern.at (Point.create -1. 0. 0.)) .= Color.black |> Assert.True
    (p |> Pattern.at (Point.create -1.1 0. 0.)) .= Color.white |> Assert.True
    
[<Fact>]
let ```lighting wiht a pattern applied``() =
    let m =
        Material.standard
        |> Material.withAmbient 1.
        |> Material.withDiffuse 0.
        |> Material.withSpecular 0.
        |> Material.withPattern (Pattern.stripes (Color.create 1. 1. 1.) (Color.create 0. 0. 0.))

    let eyev = Vector.create 0. 0. -1.
    let normalv = Vector.create 0. 0. -1.
    let light = Light.create (Color.create 1. 1. 1.) (Point.create 0. 0. -10.)
    let c1 = Object.lighting m light (Point.create 0.9 0. 0.) eyev normalv false (Object.sphere())
    let c2 = Object.lighting m light (Point.create 1.1 0. 0.) eyev normalv false (Object.sphere())
    c1 .= (Color.create 1. 1. 1.) |> Assert.True
    c2 .= (Color.create 0. 0. 0.) |> Assert.True
    
[<Fact>]
let ``stripes wih an object transformation``() =

    let o =
        Object.sphere()
        |> Object.transform (Scaling(2., 2., 2.))

    let p = Pattern.stripes Color.white Color.black

    let c = Object.pattern p o (Point.create 1.5 0. 0.)
    c .= Color.white |> Assert.True
    
[<Fact>]
let ``stripes with a pattern transformation``() =

    let o = Object.sphere()
    let p =
        Pattern.stripes Color.white Color.black
        |> Pattern.transform (Scaling(2., 2., 2.))

    let c = Object.pattern p o (Point.create 1.5 0. 0.)
    c .= Color.white |> Assert.True
    
[<Fact>]
let ``stripes with a both an object and pattern transformation``() =

    let o =
        Object.sphere()
        |> Object.transform (Scaling(2., 2., 2.))

    let p =
        Pattern.stripes Color.white Color.black
        |> Pattern.transform (Translation(0.5, 5., 5.))

    let c = Object.pattern p o (Point.create 2.5 0. 0.)
    c .= Color.white |> Assert.True


[<Fact>]
let ``a gradient linearly interpolats between colors``() =
    let pattern = Pattern.gradient Color.white Color.black
    (pattern |> Pattern.at (Point.create 0. 0. 0.)) .= Color.white |> Assert.True
    (pattern |> Pattern.at (Point.create 0.25 0. 0.)) .= Color.create 0.75 0.75 0.75 |> Assert.True
    (pattern |> Pattern.at (Point.create 0.5 0. 0.)) .= Color.create 0.5 0.5 0.5 |> Assert.True
    (pattern |> Pattern.at (Point.create 0.75 0. 0.)) .= Color.create 0.25 0.25 0.25|> Assert.True

[<Fact>]
let ``a ring should extend in both x and z``() =
    let pattern = Pattern.rings Color.white Color.black
    (pattern |> Pattern.at (Point.create 0. 0. 0.)) .= Color.white |> Assert.True
    (pattern |> Pattern.at (Point.create 1. 0. 0.)) .= Color.black|> Assert.True
    (pattern |> Pattern.at (Point.create 0. 0. 1.)) .= Color.black |> Assert.True
    (pattern |> Pattern.at (Point.create 0.708 0. 0.708)) .= Color.black |> Assert.True



[<Fact>]
let ``checkers should repeat in x``() = 
    let pattern = Pattern.checkers Color.white Color.black
    (pattern |> Pattern.at (Point.create 0. 0. 0.)) .= Color.white |> Assert.True
    (pattern |> Pattern.at (Point.create 0.99 0. 0.)) .= Color.white|> Assert.True
    (pattern |> Pattern.at (Point.create 1.01 0. 0.)) .= Color.black |> Assert.True

[<Fact>]
let ``checkers should repeat in y``() =
    let pattern = Pattern.checkers Color.white Color.black
    (pattern |> Pattern.at (Point.create 0. 0. 0.)) .= Color.white |> Assert.True
    (pattern |> Pattern.at (Point.create 0. 0.99 0.)) .= Color.white|> Assert.True
    (pattern |> Pattern.at (Point.create 0. 1.01 0.)) .= Color.black |> Assert.True

[<Fact>]
let ``checkers should repeat in z``() =
    let pattern = Pattern.checkers Color.white Color.black
    (pattern |> Pattern.at (Point.create 0. 0. 0.)) .= Color.white |> Assert.True
    (pattern |> Pattern.at (Point.create 0. 0. 0.99)) .= Color.white|> Assert.True
    (pattern |> Pattern.at (Point.create 0. 0. 1.01)) .= Color.black |> Assert.True