using Ecard.Models;
using Ecard.Services;
using Ecard.Validation;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ecard.Mvc.Models.SecondKillSets
{
    public class EditSecondKillSet : ViewModelBase, IValidator
    {
         [Dependency, NoRender]
        public ISecondKillSetService SecondKillSetService { get; set; }

        [Dependency, NoRender]
         public SecondKillSet secondKillSet { get; set; }
        SecondKillSet GetSite()
        {
            return secondKillSet ?? InnerDto;
        }
        [NoRender]
        public SecondKillSet InnerDto { get; set; }

        public EditSecondKillSet()
        {
            InnerDto = new SecondKillSet();
        }
        private Bounded _state;
        public Bounded State
        {
            get
            {
                if (_state == null)
                {
                    _state = Bounded.Create<SecondKillSet>("state", GetSite().state);
                }
                return _state;
            }
            set { _state = value; }
        }
        public string startTime
        {
            get;
            set;
        }

        public string endTime
        {
            get;
            set;
        }

        public void Ready()
        {
            secondKillSet = SecondKillSetService.GetFirst();
            if (secondKillSet != null)
            {
                startTime = secondKillSet.startTime.HasValue?secondKillSet.startTime.Value.ToString():"";
                endTime = secondKillSet.endTime.HasValue ? secondKillSet.endTime.Value.ToString() : "";
            }
        }

        public IMessageProvider Save()
        {
            var item = SecondKillSetService.GetFirst();
            if (item != null)
            {
                if (string.IsNullOrWhiteSpace(startTime))
                    item.startTime = null;
                else
                    item.startTime = Convert.ToDateTime(startTime);
                if (string.IsNullOrWhiteSpace(endTime))
                    item.endTime = null;
                else
                    item.endTime = Convert.ToDateTime(endTime);
                item.state = State.Key;
                SecondKillSetService.Update(item);
            }
            AddMessage("success", "编辑成功");
            Ready();
            return this;
        }

        public IEnumerable<ValidationError> Validate()
        {
           yield break;
        }
    }
}
