namespace RayTracer.Material

open RayTracer.Color
open RayTracer.Light
open RayTracer.Point
open RayTracer.Vector
open RayTracer.Helpers
open RayTracer.Pattern
open RayTracer.ObjectDomain
open System

module Material =

    let create ambient diffuse specular shinines color =
        {ambient = ambient; diffuse = diffuse; specular = specular; shininess = shinines; color = color; pattern = None; reflectivity = 0.; transparency = 0.; refractiveIndex = 1.}

    let standard =
        { color = Color.create 1. 1. 1.;
          ambient = 0.1;
          diffuse = 0.9;
          specular = 0.9;
          shininess = 200.;
          pattern = None;
          reflectivity = 0.;
          transparency = 0.;
          refractiveIndex = 1.; }

    let withAmbient a m = { m with ambient = a }
    let withDiffuse d m = { m with diffuse = d }
    let withSpecular s m = { m with specular = s }
    let withShininess s m = { m with shininess = s }
    let withColor c m = { m with color = c }
    let withPattern p m = { m with pattern = Some(p) }
    let withReflectivity r m = { m with reflectivity = r }
    let withTransparency t m = { m with transparency = t }
    let WithrefractiveIndex ri m = { m with refractiveIndex = ri }

    let toGlass m =
        m
        |> WithrefractiveIndex 1.5
        |> withTransparency 1.

    let superShiny m =
        m
        |> withSpecular 1.
        |> withShininess 300.
