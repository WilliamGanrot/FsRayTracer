module WorldTests

open System
open RayTracer.Vector
open RayTracer.Point
open RayTracer.Helpers
open RayTracer.Light
open RayTracer.Material
open RayTracer.Color
open RayTracer.World

open Xunit

[<Fact>]
let ``creating a world``  () =
    let w = World.empty
    w

 
