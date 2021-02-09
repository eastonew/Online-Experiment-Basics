using MainEnvironment.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
                 Pedestals = new PedestalModel[1]
                 {
                     new PedestalModel()
                     {
                          Position = new Vector3(0,0,0),
                           Scale = new Vector3(3,0.5f,3),
                            Sculpture = new SculptureModel()
                            {
                                Shapes = new List<ComponentModel>()
                            }
                     }
                }
                 }
                }
            };

            for(int i = 0; i < 100; i++)
            {
               var component = new ComponentModel()
                {
                    Position = new Vector3(0, 0.01f * i, 0.02f * i),
                    Shape = new ShapeModel()
                    {
                        BaseShape = ShapeModel.ShapeModelBaseEnum.Sphere,
                        Scale = new Vector3(0.05f, 0.05f, 0.05f)
                    }
                };

                model.Rooms[0].Pedestals[0].Sculpture.Shapes.Add(component);
            }

           string val = JsonConvert.SerializeObject(model);
        }
    }
}
