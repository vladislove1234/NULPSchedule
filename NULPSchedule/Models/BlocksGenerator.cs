using System;
using System.Collections.Generic;
using System.Linq;
using NULPSchedule.Models.Enums;
using NULPSchedule.Models.Mocks;
using Xamarin.Forms;
using Xamarin.Forms.PancakeView;

namespace NULPSchedule.Models
{
    public static class BlocksGenerator
    {
        private static Color AccentColor => Color.FromRgb(0, 49, 85);
        private static string MainFont => "Montserrat";
        private static Frame GenerateFrame(List<Lesson> lessons)
        {
            bool oneLayer = lessons.TrueForAll(x => x.TextRepeatableType.Contains("full")
                && x.RepeatableType != RepeatableType.Full_chys
                && x.RepeatableType != RepeatableType.Full_znam);
            if (lessons.Count() == 0)
                throw new Exception("lesson count = 0");
            var mainFrame = new Frame()
            {
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Margin = 5,
                Padding = 0,
                CornerRadius = 22,
                HeightRequest = oneLayer ? 120 : 240
                , BorderColor = AccentColor
            };
            var mainGrid = new Grid()
            {
                RowDefinitions = new RowDefinitionCollection()
                {
                    new RowDefinition(){ Height = GridLength.Star}
                }
            };
            var lessonsGrid = new Grid()
            {
                Padding = 0,
                ColumnSpacing = 0,
                RowSpacing = 0
            };
            
            var fullOrPartLesson = lessons.Where(x => x.RepeatableType.ToString().ToLower().Contains("full")).FirstOrDefault();
            if(fullOrPartLesson != null)
            {
                switch (fullOrPartLesson.RepeatableType)
                {
                    case RepeatableType.Full:
                        lessonsGrid.Children.Add(GetLessonFrame(fullOrPartLesson));
                        mainGrid.Children.Add(lessonsGrid, 0, 0);
                        mainFrame.Content = mainGrid;
                        return mainFrame;
                    case RepeatableType.Full_chys:
                    case RepeatableType.Full_znam:
                        lessonsGrid.RowDefinitions = new RowDefinitionCollection()
                        {
                            new RowDefinition(){ Height = GridLength.Star},
                            new RowDefinition(){ Height = GridLength.Star}
                        };
                        var frame = GetLessonFrame(fullOrPartLesson);//add gesturerecogniser
                        if (fullOrPartLesson.RepeatableType == RepeatableType.Full_chys)// for znam elements
                        {
                            lessonsGrid.Children.Add(frame, 0, 0);
                            var similarLesson = lessons.Where(x => x.RepeatableType == RepeatableType.Full_znam).FirstOrDefault();
                            if (similarLesson != null || lessons.Count == 1)
                            {
                                lessonsGrid.Children.Add(similarLesson != null ? GetLessonFrame(similarLesson) : GetEmptyViewFrame(RepeatableType.Full_znam,lessons[0].Number), 0, 1);
                                mainGrid.Children.Add(lessonsGrid, 0, 0);
                                mainFrame.Content = mainGrid;
                                return mainFrame;
                            }
                            else// add elements of znam
                            {
                                var znamGrid = new Grid()
                                {
                                    ColumnDefinitions = new ColumnDefinitionCollection()
                                    {
                                        new ColumnDefinition(){Width = GridLength.Star},
                                        new ColumnDefinition(){Width = GridLength.Star}
                                    }
                                };
                                var group1znam = lessons.Where(x => x.RepeatableType == RepeatableType.Group1_znam).FirstOrDefault();
                                var group2znam = lessons.Where(x => x.RepeatableType == RepeatableType.Group2_znam).FirstOrDefault();
                                znamGrid.Children.Add(group1znam != null ? GetLessonFrame(group1znam) : GetEmptyViewFrame(RepeatableType.Group1_znam, lessons[0].Number), 0, 0);
                                znamGrid.Children.Add(group2znam != null ? GetLessonFrame(group2znam) : GetEmptyViewFrame(RepeatableType.Group2_znam, lessons[0].Number), 1, 0);
                                lessonsGrid.Children.Add(znamGrid, 0, 1);
                            }
                        }
                        else//for chys elements
                        {
                            lessonsGrid.Children.Add(frame, 0, 1);
                            var similarLesson = lessons.Where(x => x.RepeatableType == RepeatableType.Full_chys).FirstOrDefault();
                            if (similarLesson != null || lessons.Count == 1)
                            {
                                lessonsGrid.Children.Add(similarLesson != null ? GetLessonFrame(similarLesson) : GetEmptyViewFrame(RepeatableType.Full_chys, lessons[0].Number), 0, 0);
                                mainGrid.Children.Add(lessonsGrid, 0, 0);
                                mainFrame.Content = mainGrid;
                                return mainFrame;
                            }
                            else// add elements of group 2
                            {
                                var znamGrid = new Grid()
                                {
                                    ColumnDefinitions = new ColumnDefinitionCollection()
                                    {
                                        new ColumnDefinition(){Width = GridLength.Star},
                                        new ColumnDefinition(){Width = GridLength.Star}
                                    }
                                };
                                var group1chys = lessons.Where(x => x.RepeatableType == RepeatableType.Group1_chys).FirstOrDefault();
                                var group2chys = lessons.Where(x => x.RepeatableType == RepeatableType.Group2_chys).FirstOrDefault();
                                znamGrid.Children.Add(group1chys != null ? GetLessonFrame(group1chys) : GetEmptyViewFrame(RepeatableType.Group1_chys, lessons[0].Number), 0, 0);
                                znamGrid.Children.Add(group2chys != null ? GetLessonFrame(group2chys) : GetEmptyViewFrame(RepeatableType.Group2_chys, lessons[0].Number), 1, 0);
                                lessonsGrid.Children.Add(znamGrid, 0, 0);
                            }
                        }
                        break;
                    case RepeatableType.Group1_full:
                    case RepeatableType.Group2_full:
                        lessonsGrid.ColumnDefinitions = new ColumnDefinitionCollection()
                        {
                            new ColumnDefinition(){ Width = GridLength.Star},
                            new ColumnDefinition(){ Width = GridLength.Star}
                        };
                        var groupFrame = GetLessonFrame(fullOrPartLesson);//add gesturerecogniser
                        if (fullOrPartLesson.RepeatableType == RepeatableType.Group1_full)// elements for group 1
                        {
                            lessonsGrid.Children.Add(groupFrame, 0, 0);
                            var similarLesson = lessons.Where(x => x.RepeatableType == RepeatableType.Group2_full).FirstOrDefault();
                            if (similarLesson != null || lessons.Count == 1)
                            {
                                lessonsGrid.Children.Add(similarLesson != null ? GetLessonFrame(similarLesson) : GetEmptyViewFrame(RepeatableType.Group2_full, lessons[0].Number), 1, 0);
                                mainGrid.Children.Add(lessonsGrid, 0, 0);
                                mainFrame.Content = mainGrid;
                                return mainFrame;
                            }
                            else// add elements of group 2
                            {
                                var group1Grid = new Grid()
                                {
                                    RowDefinitions = new RowDefinitionCollection()
                                    {
                                        new RowDefinition(){ Height = GridLength.Star},
                                        new RowDefinition(){ Height = GridLength.Star}
                                    }
                                };
                                var group2_znam = lessons.Where(x => x.RepeatableType == RepeatableType.Group2_znam).FirstOrDefault();
                                var group2_chys = lessons.Where(x => x.RepeatableType == RepeatableType.Group2_chys).FirstOrDefault();
                                group1Grid.Children.Add(group2_chys != null ? GetLessonFrame(group2_chys) : GetEmptyViewFrame(RepeatableType.Group2_chys, lessons[0].Number), 0, 0);
                                group1Grid.Children.Add(group2_znam != null ? GetLessonFrame(group2_chys) : GetEmptyViewFrame(RepeatableType.Group2_znam, lessons[0].Number), 0, 1);
                                lessonsGrid.Children.Add(group1Grid, 1, 0);
                            }
                        }
                        else//elements for group 2
                        {
                            lessonsGrid.Children.Add(groupFrame, 1, 0);
                            var similarLesson = lessons.Where(x => x.RepeatableType == RepeatableType.Group1_full).FirstOrDefault();
                            if (similarLesson != null || lessons.Count == 1)
                            {
                                lessonsGrid.Children.Add(similarLesson != null ? GetLessonFrame(similarLesson) : GetEmptyViewFrame(RepeatableType.Group1_full, lessons[0].Number), 0, 0);
                                mainGrid.Children.Add(lessonsGrid, 0, 0);
                                mainFrame.Content = mainGrid;
                                return mainFrame;
                            }
                            else// add elements for group 1
                            {
                                var group1Grid = new Grid()
                                {
                                    RowDefinitions = new RowDefinitionCollection()
                                    {
                                        new RowDefinition(){ Height = GridLength.Star},
                                        new RowDefinition(){ Height = GridLength.Star}
                                    }
                                };
                                var group1_znam = lessons.Where(x => x.RepeatableType == RepeatableType.Group1_znam).FirstOrDefault();
                                var group1_chys = lessons.Where(x => x.RepeatableType == RepeatableType.Group1_chys).FirstOrDefault();
                                group1Grid.Children.Add(group1_chys != null ? GetLessonFrame(group1_chys) : GetEmptyViewFrame(RepeatableType.Group1_chys, lessons[0].Number), 0, 0);
                                group1Grid.Children.Add(group1_znam != null ? GetLessonFrame(group1_znam) : GetEmptyViewFrame(RepeatableType.Group1_znam, lessons[0].Number), 0, 1);
                                lessonsGrid.Children.Add(group1Grid, 0, 0);
                            }
                        }
                        mainGrid.Children.Add(lessonsGrid, 0, 0);
                        mainFrame.Content = mainGrid;
                        return mainFrame;
                }
            }
            else
            {
                var group1_znam = lessons.Where(x => x.RepeatableType == RepeatableType.Group1_znam).FirstOrDefault();
                var group1_chys = lessons.Where(x => x.RepeatableType == RepeatableType.Group1_chys).FirstOrDefault();
                var group2_znam = lessons.Where(x => x.RepeatableType == RepeatableType.Group2_znam).FirstOrDefault();
                var group2_chys = lessons.Where(x => x.RepeatableType == RepeatableType.Group2_chys).FirstOrDefault();

                lessonsGrid.ColumnDefinitions = new ColumnDefinitionCollection()
                {
                    new ColumnDefinition(){Width = GridLength.Star},
                    new ColumnDefinition(){Width = GridLength.Star}
                };
                lessonsGrid.RowDefinitions = new RowDefinitionCollection()
                {
                    new RowDefinition(){Height = GridLength.Star},
                    new RowDefinition(){Height = GridLength.Star}
                };
                lessonsGrid.Children.Add(group1_znam != null ? GetLessonFrame(group1_znam) : GetEmptyViewFrame(RepeatableType.Group1_znam, lessons[0].Number), 0, 1);
                lessonsGrid.Children.Add(group1_chys != null ? GetLessonFrame(group1_chys) : GetEmptyViewFrame(RepeatableType.Group1_chys, lessons[0].Number), 0, 0);
                lessonsGrid.Children.Add(group2_znam != null ? GetLessonFrame(group2_znam) : GetEmptyViewFrame(RepeatableType.Group2_znam, lessons[0].Number), 1, 1);
                lessonsGrid.Children.Add(group2_chys != null ? GetLessonFrame(group2_chys) : GetEmptyViewFrame(RepeatableType.Group2_chys,lessons[0].Number), 1, 0);
            }
            mainGrid.Children.Add(lessonsGrid, 0, 0);
            mainFrame.Content = mainGrid;
            return mainFrame;
        }
        private static PancakeView GetEmptyViewFrame(RepeatableType type,int lessonNumber)
        {
            var pancakeView = new PancakeView()
            {
                HorizontalOptions = LayoutOptions.Fill,
                HeightRequest = 123,
                BackgroundColor = Color.White,
                CornerRadius = GetCornerRadius(type),
                Border = new Border() { Color = AccentColor, Thickness = 1 },
                Padding = 0
            };
            switch (type)
            {
                case RepeatableType.Full:
                case RepeatableType.Full_chys:
                    pancakeView.Content = new Grid()
                    {
                        ColumnDefinitions = new ColumnDefinitionCollection()
                        {
                            new ColumnDefinition(){ Width = GridLength.Star},
                            new ColumnDefinition(){ Width = GridLength.Star}
                        },
                        Children =
                        {
                            { new Label(){
                                Text = lessonNumber.ToString() + " пара",
                                Margin = new Thickness(16,4,1,1),
                                TextColor = Color.Black,
                                HorizontalTextAlignment = TextAlignment.Start,
                                VerticalTextAlignment = TextAlignment.Start,
                                FontSize = 12,
                                FontFamily = MainFont
                            },0,0 },
                            { new Label(){
                                Text = LessonsTimeManager.GetLessonTime(lessonNumber),
                                Margin = new Thickness(1,4,16,1),
                                TextColor = Color.Black,
                                HorizontalTextAlignment = TextAlignment.End,
                                VerticalTextAlignment = TextAlignment.Start,
                                FontSize = 12,
                                FontFamily = MainFont
                            },1,0 }
                        },
                        Padding = 0,
                    };
                    break;
                case RepeatableType.Group1_full:
                case RepeatableType.Group1_chys:
                    pancakeView.Content = new Label()
                    {
                        Text = lessonNumber+ " пара",
                        Margin = new Thickness(16, 4, 1, 1),
                        HorizontalTextAlignment = TextAlignment.Start,
                        VerticalTextAlignment = TextAlignment.Start,
                        TextColor = Color.Black,
                        Padding = 0,
                        FontSize = 12,
                        FontFamily = MainFont
                    };
                    break;
                case RepeatableType.Group2_full:
                case RepeatableType.Group2_chys:
                    pancakeView.Content = new Label()
                    {
                        Text = LessonsTimeManager.GetLessonTime(lessonNumber),
                        Margin = new Thickness(1, 4, 16, 1),
                        HorizontalTextAlignment = TextAlignment.End,
                        TextColor = Color.Black,
                        Padding = 0,
                        FontSize = 12,
                        FontFamily = MainFont
                    };
                    break;
            }
            return pancakeView;
        }
        private static PancakeView GetLessonFrame(Lesson lesson)
        {
            bool thisWeekLesson = lesson.PairWeek == DayManager.PairWeekToday()
                || (!lesson.TextRepeatableType.Contains("znam") && !lesson.TextRepeatableType.Contains("chys"));
            var content = new Grid()
            {
                RowDefinitions = new RowDefinitionCollection()
                {
                    new RowDefinition(){Height = GridLength.Star },
                    new RowDefinition(){ Height = new GridLength(5.5,GridUnitType.Star)},
                    new RowDefinition(){Height = GridLength.Star }
                },
                Padding = 0
            };
            switch (lesson.RepeatableType)
            {
                case RepeatableType.Full:
                case RepeatableType.Full_chys:
                    content.Children.Add(new Grid()
                    {
                        ColumnDefinitions = new ColumnDefinitionCollection()
                        {
                            new ColumnDefinition(){ Width = GridLength.Star},
                            new ColumnDefinition(){ Width = GridLength.Star}
                        },
                        Children =
                        {
                            { new Label(){
                                Text = lesson.Number.ToString() + " пара",
                                Margin = new Thickness(16,4,1,1),
                                TextColor = thisWeekLesson ? Color.White : Color.Black,
                                HorizontalTextAlignment = TextAlignment.Start,
                                FontSize = 12,
                                FontFamily = MainFont
                            },0,0 },
                            { new Label(){
                                Text = LessonsTimeManager.GetLessonTime(lesson.Number),
                                Margin = new Thickness(1,4,16,1),
                                TextColor = thisWeekLesson ? Color.White : Color.Black,
                                HorizontalTextAlignment = TextAlignment.End,
                                FontSize = 12,
                                FontFamily = MainFont
                            },1,0 }
                        },
                        Padding = 0,
                    }, 0, 0);
                    break;
                case RepeatableType.Group1_full:
                case RepeatableType.Group1_chys:
                    content.Children.Add(new Label()
                    {
                        Text = lesson.Number.ToString() + " пара",
                        Margin = new Thickness(16, 4, 1, 1),
                        HorizontalTextAlignment = TextAlignment.Start,
                        TextColor = thisWeekLesson? Color.White : Color.Black,
                        Padding = 0,
                        FontSize = 12,
                        FontFamily = MainFont
                    }, 0, 0);
                    break;
                case RepeatableType.Group2_full:
                case RepeatableType.Group2_chys:
                    content.Children.Add(new Label()
                    {
                        Text = LessonsTimeManager.GetLessonTime(lesson.Number),
                        Margin = new Thickness(1, 4, 16, 1),
                        HorizontalTextAlignment = TextAlignment.End,
                        TextColor = thisWeekLesson ? Color.White : Color.Black,
                        Padding = 0,
                        FontSize = 12,
                        FontFamily = MainFont
                    }, 0, 0);
                    break;
            }
            content.Children.Add(
                new StackLayout()
                {
                    Children = {new Label()
                    {
                        Text = lesson.Name,
                        VerticalTextAlignment = TextAlignment.Center,
                        HorizontalTextAlignment = TextAlignment.Center,
                        FontFamily = "Montserrat",
                        TextColor = thisWeekLesson? Color.White : Color.Black,
                        Padding = 0
                    },
                    new Label()
                    {
                        Text = lesson.Description,
                        VerticalTextAlignment = TextAlignment.Center,
                        HorizontalTextAlignment = TextAlignment.Center,
                        FontFamily = "Montserrat",
                        FontSize = 7,
                        TextColor = thisWeekLesson ? Color.White : Color.Black,
                        Padding = 0
                    },
                    new Label(){
                        Text = lesson.Type,
                        VerticalTextAlignment = TextAlignment.Center,
                        HorizontalTextAlignment = TextAlignment.Center,
                        FontFamily = "Montserrat",
                        FontSize = 7,
                        TextColor = thisWeekLesson ? Color.White : Color.Black,
                        Padding = 0
                    }

                }
                },0,1);
            var pancakeView = new PancakeView()
            {
                HorizontalOptions = LayoutOptions.Fill,
                HeightRequest = 123,
                BackgroundColor = thisWeekLesson ? Color.FromRgb(0,49,85) : Color.White,
                CornerRadius = GetCornerRadius(lesson.RepeatableType),
                Content = content,
                Padding = 0,
                Margin = 1
            };
            return pancakeView;
        }
        private static CornerRadius GetCornerRadius(RepeatableType type)
        {
            switch (type)
            {
                case RepeatableType.Full:
                    return new CornerRadius(22);
                case RepeatableType.Full_chys:
                    return new CornerRadius(22,22,0,0);
                case RepeatableType.Full_znam:
                    return new CornerRadius(0,0,22,22);
                case RepeatableType.Group1_full:
                    return new CornerRadius(22, 0, 22, 0);
                case RepeatableType.Group1_znam:
                    return new CornerRadius(0,0,22,0);
                case RepeatableType.Group1_chys:
                    return new CornerRadius(22,0,0,0);
                case RepeatableType.Group2_full:
                    return new CornerRadius(0, 22, 0, 22);
                case RepeatableType.Group2_znam:
                    return new CornerRadius(0, 0, 0, 22);
                case RepeatableType.Group2_chys:
                    return new CornerRadius(0,22,0,0);
                default:
                    return new CornerRadius(0);
            }
        }
        public static List<Frame> GetFrames(List<Lesson> lessons)
        {
            var frames = new List<Frame>();
            for(int i = 1; i < 10; i++)
            {
                var currentLessons = lessons.Where(x => x.Number == i);
                if (currentLessons.Count() > 0)
                    frames.Add(GenerateFrame(new List<Lesson>(currentLessons)));
            }
            return frames;
        }
    }
}
