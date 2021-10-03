
namespace RayTracer.ObjectDomain

open RayTracer.Helpers
open RayTracer.RayDomain

[<AutoOpen>]
module ObjectDomain =

    type Color =
        {Red:float; Green:float; Blue:float}

        static member (+) (c1, c2) =
            { Red = c1.Red + c2.Red
              Green = c1.Green + c2.Green
              Blue = c1.Blue + c2.Blue }

        static member (-) (c1, c2) =
            { Red = c1.Red - c2.Red
              Green = c1.Green - c2.Green
              Blue = c1.Blue - c2.Blue }

        static member (*) (c1, c2) =
            { Red = c1.Red * c2.Red
              Green = c1.Green * c2.Green
              Blue = c1.Blue * c2.Blue }

        static member (*) ((c:Color), (scalar:int)) =
            { Red = c.Red * (float)scalar
              Green = c.Green * (float)scalar
              Blue = c.Blue * (float)scalar }

        static member (.=) (c1, c2 : Color) =
                FloatHelper.equal c1.Red c2.Red && FloatHelper.equal c1.Green c2.Green && FloatHelper.equal c1.Blue c2.Blue
    type Matrix =
        {dimensions: int; entries: float [,]}

        static member (*) (a: Matrix, b: Matrix) =
            let entries = 
                [for rowindex in 0..a.dimensions - 1 do
                    [for colindex in 0..b.dimensions - 1 do
                        let row = a.entries.[rowindex,*]
                        let col = b.entries.[*,colindex]
                        (Array.map2(fun x y -> x * y) row col) |> Array.sum ]]

            {a with entries = array2D entries}

        static member (.=) (a: Matrix, b: Matrix) =

            let aList = a.entries |> Seq.cast<float> |> Seq.toList
            let bList = b.entries |> Seq.cast<float> |> Seq.toList

            a.dimensions = b.dimensions && Seq.forall2(fun x y -> FloatHelper.equal x y) aList bList

    type Axis =
        | X
        | Y
        | Z

    type Transformation =
        | Translation of x:float * y:float * z:float
        | Scaling of x:float * y:float * z:float
        | Rotation of axis:Axis * degrees:float
        | Shering of xy:float * xz:float * yx:float * yz:float * zx:float * zy:float
        | TransformationGroup of transformations:Transformation list
        | Matrix of Matrix

    type PatternType =
        | Stripes
        | Gradient
        | Rings
        | Checkers

    type Pattern = { transform: Matrix; patternType: PatternType; a: Color; b: Color; }

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


    [<CustomEquality; CustomComparison>]
    type Shape =
        | Sphere
        | Plane
        | Cube
        | Cylinder of minimum: float * maximum: float * closed: bool
        | Cone of minimum: float * maximum: float * closed: bool
        | Group of Object list
        | Traingle of p1: Point * p2: Point * p3: Point * e1: Vector * e2: Vector * normal: Vector
        | SmoothTraingle of p1: Point * p2: Point * p3: Point * e1: Vector * e2: Vector * n: Vector * n2: Vector * n3: Vector
        
        override x.Equals(y) =
            (match y with :? Shape -> true | _ -> false)
 
        override x.GetHashCode() = hash x

        interface System.IComparable with
          member x.CompareTo yobj =
              match yobj with
              | :? Shape as y -> compare x y
              | _ -> invalidArg "yobj" "cannot compare values of different types"
 
    and Object =
        { transform: Matrix;
          transformInverse: Matrix;
          material: Material;
          shape: Shape;
          id: int;
          localIntersect: Object -> Ray -> Intersection list;
          localNormalAt: Shape -> Point -> Intersection Option -> Vector;
          bounds: BoundingBox}

        //compares object without id
        static member (.=.) (p, v : Object) =
           p.transform = v.transform && p.transformInverse = v.transformInverse && p.material = v.material && p.shape = v.shape

        static member (<>.) (p, v : Object) =
           p.transform <> v.transform || p.transformInverse <> v.transformInverse && p.material <> v.material || p.shape <> v.shape


    and Intersection =
        { t: float; object: Object; uv: (float * float) Option }

        static member (.=.) (p: Intersection, v: Intersection) =
            p.object .=. v.object && FloatHelper.equal p.t v.t

    and BoundingBox = {min: Point; max: Point;(* objects: Object list*)}

    type Computation =
        { t: float;
          object: Object;
          point:Point;
          overPoint: Point;
          eyev: Vector;
          inside: bool;
          normalv: Vector;
          reflectv: Vector;
          n1: float;
          n2: float;
          underPoint: Point}



