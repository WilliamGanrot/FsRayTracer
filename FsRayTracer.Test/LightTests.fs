module LightTests

open System
open RayTracer.Vector
open RayTracer.Point
open RayTracer.Helpers
open RayTracer.Light
open RayTracer.Color

open Xunit

[<Fact>]
let ``a light has a position ant intensity`` () =
    let i = Color.create 1. 1. 1.
    let p = Point.create 0. 0. 0.

    let light = Light.create i p

    (Point.equal light.poistion p) |> Assert.True
    light.intensity .= i |> Assert.True
