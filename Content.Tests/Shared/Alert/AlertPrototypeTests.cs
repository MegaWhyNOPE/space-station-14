﻿using System.IO;
using Content.Shared.Alert;
using NUnit.Framework;
using Robust.Shared.Interfaces.Log;
using Robust.Shared.IoC;
using Robust.Shared.Log;
using Robust.Shared.Utility;
using Robust.UnitTesting;
using YamlDotNet.RepresentationModel;

namespace Content.Tests.Shared.Alert
{
    [TestFixture, TestOf(typeof(AlertPrototype))]
    public class AlertPrototypeTests : RobustUnitTest
    {
        private const string PROTOTYPE = @"- type: alert
  alertType: HumanHealth
  category: Health
  icon: /Textures/Interface/Alerts/Human/human.rsi/human.png
  name: Health
  description: ""[color=green]Green[/color] good. [color=red]Red[/color] bad.""
  minSeverity: 0
  maxSeverity: 6";


        [Test]
        public void TestAlertKey()
        {
            Assert.That(new AlertKey(AlertType.HumanHealth, null), Is.Not.EqualTo(AlertKey.ForCategory(AlertCategory.Health)));
            Assert.That((new AlertKey(null, AlertCategory.Health)), Is.EqualTo(AlertKey.ForCategory(AlertCategory.Health)));
            Assert.That((new AlertKey(AlertType.Buckled, AlertCategory.Health)), Is.EqualTo(AlertKey.ForCategory(AlertCategory.Health)));
        }


        [TestCase(0, "/Textures/Interface/Alerts/Human/human.rsi/human0.png")]
        [TestCase(null, "/Textures/Interface/Alerts/Human/human.rsi/human0.png")]
        [TestCase(1, "/Textures/Interface/Alerts/Human/human.rsi/human1.png")]
        [TestCase(6, "/Textures/Interface/Alerts/Human/human.rsi/human6.png")]
        [TestCase(7, "/Textures/Interface/Alerts/Human/human.rsi/human6.png")]
        public void GetsIconPath(short? severity, string expected)
        {

            var alert = GetTestPrototype();
            Assert.That(alert.GetIconPath(severity), Is.EqualTo(expected));
        }

        private AlertPrototype GetTestPrototype()
        {
            using (TextReader stream = new StringReader(PROTOTYPE))
            {
                var yamlStream = new YamlStream();
                yamlStream.Load(stream);
                var document = yamlStream.Documents[0];
                var rootNode = (YamlSequenceNode) document.RootNode;
                var proto = (YamlMappingNode) rootNode[0];
                var newReagent = new AlertPrototype();
                newReagent.LoadFrom(proto);
                return newReagent;
            }
        }
    }
}
