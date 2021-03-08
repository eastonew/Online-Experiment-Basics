using MainEnvironment.Core;
using MainEnvironment.Core.Enums;
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
                Questionnaire = new QuestionnaireModel()
                {
                    Questions = new QuestionModel[]
                    {
                        new QuestionModel()
                        {
                            QuestionType = QuestionModel.QuestionTypeEnum.TrueFalse,
                            FalseAnswerText = "No",
                            TrueAnswerText = "Yes",
                            Prefix = "Can you see this question?",
                        }
                    }
                },
                Rooms = new RoomModel[1]
                {
                    new RoomModel()
                    {
                         Order = 0,
                         ShowDoorCaps = true,
                         Depth = 50,
                         Height = 5,
                         Width = 50,
                         Pedestals = new PedestalModel[150]
                    }
                }
            };

            Random rand = new Random();
            for (int i = 0; i < 5; i++)
            {
                
                var sculptureJson = File.ReadAllText($@"H:\Sculpturs\MainExperimentTest\SmallItems\Item{i}.json");
                var sculpture = JsonConvert.DeserializeObject<SculptureExport>(sculptureJson);
                model.Rooms[0].Pedestals[i] = new PedestalModel()
                {
                    Position = new Vector3(rand.Next(0,50)-25, 0, rand.Next(0,50)-25),
                    Scale = new Vector3(3, 0.5f, 3),
                    Sculpture = new SculptureModel()
                    {
                        Shapes = sculpture.CalculatedPoints.Select(p => new ComponentModel()
                        {
                             Position = p,
                              Shape = new ShapeModel()
                              {
                                  BaseShape = ShapeModel.ShapeModelBaseEnum.Sphere,
                                  Scale = new Vector3(0.05f, 0.05f, 0.05f)
                              }
                        }).ToList(),
                        AvailableActions = ActionEnum.Translation & ActionEnum.RotationY
                    }
                };
            }
            

            //for(int i = 0; i < 100; i++)
            //{
            //   var component = new ComponentModel()
            //    {
            //        Position = new Vector3(0, 0.01f * i, 0.02f * i),
            //        Shape = new ShapeModel()
            //        {
            //            BaseShape = ShapeModel.ShapeModelBaseEnum.Sphere,
            //            Scale = new Vector3(0.05f, 0.05f, 0.05f)
            //        }
            //    };

            //    model.Rooms[0].Pedestals[0].Sculpture.Shapes.Add(component);
            //}

           string val = JsonConvert.SerializeObject(model);
        }
    }
}
