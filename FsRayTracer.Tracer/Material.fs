namespace RayTracer.Material

open RayTracer.Color
open RayTracer.Light
open RayTracer.Point
open RayTracer.Vector
open RayTracer.Helpers
open RayTracer.Pattern
open System

[<AutoOpen>]
module Domain =
    type Material =
        { color: Color;
          ambient:float;
          diffuse:float;
          specular: float;
          shininess: float;
          pattern: Pattern Option;
          reflectivity: float;
          transparency: float;
          refractiveIndex: float; }

          static member (.=) (m : Material, m2: Material) =
            m.color .= m2.color &&
            FloatHelper.equal m.ambient m2.ambient &&
            FloatHelper.equal m.diffuse m2.diffuse &&
            FloatHelper.equal m.specular m2.shininess &&
            FloatHelper.equal m.reflectivity m2.reflectivity &&
            FloatHelper.equal m.transparency m2.transparency &&
            FloatHelper.equal m.refractiveIndex m2.refractiveIndex &&
            m.pattern = m2.pattern

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
