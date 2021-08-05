using MainEnvironment.Core;
using MainEnvironment.Core.Enums;
using MainEnvironment.Core.Models;
using MainEnvironment.Database;
using MainEnvironment.Web.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;

namespace MainEnvironment.ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            SceneModel model = new SceneModel()
            {
                AvailableActions = ActionEnum.Translation | ActionEnum.RotationY | ActionEnum.Flying | ActionEnum.UserScaling,
                AllTags = new TagModel[]
                {
                    new TagModel(){ Id = Guid.NewGuid(), Name = "Test Tag 1" },
                    new TagModel(){ Id = Guid.NewGuid(), Name = "Test Tag 2" },
                    new TagModel(){ Id = Guid.NewGuid(), Name = "Test Tag 3" },
                    new TagModel(){ Id = Guid.NewGuid(), Name = "Test Tag 4" },
                    new TagModel(){ Id = Guid.NewGuid(), Name = "Test Tag 5" },
                    new TagModel(){ Id = Guid.NewGuid(), Name = "Test Tag 6" },
                    new TagModel(){ Id = Guid.NewGuid(), Name = "Test Tag 7" },
                    new TagModel(){ Id = Guid.NewGuid(), Name = "Test Tag 8" },
                    new TagModel(){ Id = Guid.NewGuid(), Name = "Test Tag 9" },
                    new TagModel(){ Id = Guid.NewGuid(), Name = "Test Tag 10" },
                },
                Regions = null,
                 Sculptures = new SculptureModel[]
                 {
                     new SculptureModel()
                     {
                          GroupId =3,
                           SculptureId = Guid.Parse("c459d9a1-157f-45ae-9715-7372a779e9c7"),
                     },
                     new SculptureModel()
                     {
                          GroupId =3,
                           SculptureId = Guid.Parse("3cded1d5-55df-4829-88d0-a5b271b21bca"),
                     },
                     new SculptureModel()
                     {
                          GroupId =3,
                           SculptureId = Guid.Parse("a6e14e53-5f4a-4f24-9b40-bdface0b5f1e"),
                     },
                     new SculptureModel()
                     {
                          GroupId =3,
                           SculptureId = Guid.Parse("896ec697-9f45-4090-8b08-f429fff20841"),
                     },
                     new SculptureModel()
                     {
                          GroupId =3,
                           SculptureId = Guid.Parse("21525d2a-1829-4d73-98f1-f33266a7cda9"),
                     },
                     new SculptureModel()
                     {
                          GroupId =3,
                           SculptureId = Guid.Parse("c26c1f5f-8a42-4989-bccd-4e413f067ce8"),
                     },


                     new SculptureModel()
                     {
                          GroupId =4,
                           SculptureId = Guid.Parse("e4c9587e-74fc-46b5-b27c-bf338732eec5"),
                     },
                     new SculptureModel()
                     {
                          GroupId =4,
                           SculptureId = Guid.Parse("d4114887-967c-480d-8aa9-ca7e89a13c6b"),
                     },
                     new SculptureModel()
                     {
                          GroupId =4,
                           SculptureId = Guid.Parse("e75ca99b-3108-4c00-a654-d1c178a1254d"),
                     },
                     new SculptureModel()
                     {
                          GroupId =4,
                           SculptureId = Guid.Parse("c6d4e927-3082-45d5-8065-1e0ac0e5255b"),
                     },
                     new SculptureModel()
                     {
                          GroupId =4,
                           SculptureId = Guid.Parse("5b4474db-8291-4dca-a3d1-3a37b6bae371"),
                     },
                     new SculptureModel()
                     {
                          GroupId =4,
                           SculptureId = Guid.Parse("f3b0618c-5002-40d9-ad16-5e84ff752b08"),
                     }
                 },
            };
           string val = JsonConvert.SerializeObject(model);
        }
    }
}
