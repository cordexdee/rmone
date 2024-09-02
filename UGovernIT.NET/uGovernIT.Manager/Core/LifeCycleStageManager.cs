using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;
using uGovernIT.Helpers;
using uGovernIT.Manager.Core; //added by amar
using System.Web; //added by amar
namespace uGovernIT.Manager.Core 
{
    public class LifeCycleStageManager:ManagerBase<LifeCycleStage>
    {        
        public LifeCycleStageManager(ApplicationContext context):base(context)
        {

        }
        
        public  void DeleteLifeCycleStage(string module, long deleteStageID)
        {

            LifeCycleStage lifeCycleStage = new LifeCycleStage();
            RequestRoleWriteAccessManager RequestRoleWriteAccessManager = new RequestRoleWriteAccessManager(this.dbContext);

            LifeCycleStageManager objLifeCycleStageManager = new LifeCycleStageManager(this.dbContext);
            LifeCycleManager lcHelper = new Manager.LifeCycleManager(this.dbContext);


            List<LifeCycle> lifeCycles = lcHelper.LoadLifeCycleByModule(module);
            LifeCycle currentLifeCycle = lifeCycles.Where(m => m.ID == 0).FirstOrDefault();
            LifeCycleStage deleteLifeCycleStage = currentLifeCycle.Stages.FirstOrDefault(x => x.ID == deleteStageID);
            
            //List<LifeCycleStage> lifeCycles = lcHelper.LoadLifeCycleByModule(module);
            //LifeCycleStage currentLifeCycle = lifeCycles.Where(m => m.ID == 0).FirstOrDefault();
            //LifeCycleStage deleteLifeCycleStage = currentLifeCycle.Stages.FirstOrDefault(x => x.ID == deleteStageID);

            //long nextID = currentLifeCycle.Stages
            //            .Where(x => x.StageStep.Equals(deleteLifeCycleStage.StageStep + 1))
            //            .Select(s => s.ID).FirstOrDefault();

            //long prevID = currentLifeCycle.Stages
            //            .Where(x => x.StageStep.Equals(deleteLifeCycleStage.StageStep - 1))
            //            .Select(s => s.ID).FirstOrDefault();


            int nextStagesID = currentLifeCycle.Stages
                       .Where(x => x.StageStep.Equals(deleteLifeCycleStage.StageStep + 1))
                       .Select(s => s.StageStep).FirstOrDefault();

            int prevStagesID = currentLifeCycle.Stages
                        .Where(x => x.StageStep.Equals(deleteLifeCycleStage.StageStep - 1))
                        .Select(s => s.StageStep).FirstOrDefault();
            
            ////approval Dependancy
            var approvalDependancy = currentLifeCycle.Stages.Where(x => x.ApprovedStage != null && x.ApprovedStage.ID.Equals(deleteStageID));
            if (approvalDependancy.Count() > 0)
            {
                foreach (var item in approvalDependancy)
               {
                    if (nextStagesID != item.StageStep)
                    {

                        item.StageApprovedStatus = nextStagesID ;
                        lifeCycleStage = item;
                        objLifeCycleStageManager.Update(lifeCycleStage);
                    }

                    //if (nextID != item.ID)
                    //{
                    //    item.ApprovedStage.ID = nextID;
                    //    item.StageApprovedStatus = item.ApprovedStage.StageStep;

                    //    lifeCycleStage = item;
                    //    objLifeCycleStageManager.Update(lifeCycleStage);
                    //}

                }
            }

            ////return Dependancy
            var returnDependancy = currentLifeCycle.Stages.Where(x => x.ReturnStage != null && x.ReturnStage.ID.Equals(deleteStageID));
            if (returnDependancy.Count() > 0)
            {
                foreach (var item in returnDependancy)
               {
                    if (prevStagesID != item.StageStep)
                    {
                        item.StageReturnStatus = prevStagesID ;
                        
                        lifeCycleStage = item;
                        objLifeCycleStageManager.Update(lifeCycleStage);
                    }

                    //if (prevID != item.ID)
                    //{

                    //    item.ReturnStage.ID = prevID;
                    //    item.StageReturnStatus = item.ReturnStage.StageStep;

                    //    lifeCycleStage = item;
                    //    objLifeCycleStageManager.Update(lifeCycleStage);
                    //}
                }
            }

            ////reject Dependancy
            var rejectDependancy = currentLifeCycle.Stages.Where(x => x.RejectStage != null && x.RejectStage.ID.Equals(deleteStageID));
            if (rejectDependancy.Count() > 0)
            {
                if (deleteLifeCycleStage.StageStep.Equals(currentLifeCycle.Stages.Max(x => x.StageStep)))
                {
            //        //Last Step
                    foreach (var item in rejectDependancy)
                    {
                        if (prevStagesID != item.StageStep)
                        {
                            item.StageRejectedStatus = prevStagesID ;
                            lifeCycleStage = item;
                            objLifeCycleStageManager.Update(lifeCycleStage);
                        }

                        //if (prevID != item.ID)
                        //{
                        //    item.RejectStage.ID = prevID;
                        //    item.StageReturnStatus = item.RejectStage.StageStep;
                        //    lifeCycleStage = item;

                        //    objLifeCycleStageManager.Update(lifeCycleStage);
                        //}
                    }
                }
                else
                {
                    foreach (var item in rejectDependancy)
                    {
                        //SPListItem spListItem = SPListHelper.GetSPListItem(DatabaseObjects.Lists.ModuleStages, item.ID);
                        if (nextStagesID != item.StageStep)
                        {
                            item.StageRejectedStatus = nextStagesID ;
                            lifeCycleStage = item;
                            objLifeCycleStageManager.Update(lifeCycleStage);
                        }

                        //if (nextID != item.ID)
                        //{
                        //    item.RejectStage.ID = nextID;
                        //    lifeCycleStage = item;
                        //    objLifeCycleStageManager.Update(lifeCycleStage);
                        //}
                    }
                }
            }
            
            ////Change stage step #
            var changeStep = currentLifeCycle.Stages.Where(x => x.StageStep > deleteLifeCycleStage.StageStep);
            foreach (var item in changeStep)
            {                
                     item.StageStep = item.StageStep - 1;
                     lifeCycleStage = item;
                     objLifeCycleStageManager.Update(lifeCycleStage);                
            }

            ////delete Item
            //SPListItem deleteItem = SPListHelper.GetSPListItem(DatabaseObjects.Lists.ModuleStages, deleteStageID, SPContext.Current.Web);

            var  deleteItem = currentLifeCycle.Stages.Where(x => x.ID == deleteLifeCycleStage.ID);

            //#region Delete / Update From RequestRoleWriteAccess
                       string ModuleStages_ModuleName = deleteLifeCycleStage.ModuleNameLookup;
            int ModuleStages_ModuleStep =deleteLifeCycleStage.StageStep;
           
            List<ModuleRoleWriteAccess> ModuleRoleWriteAccess = RequestRoleWriteAccessManager.Load();
            List<ModuleRoleWriteAccess> itemsToDelete = ModuleRoleWriteAccess.Where(s => s.StageStep == ModuleStages_ModuleStep && s.ModuleNameLookup == ModuleStages_ModuleName).ToList();
            List<ModuleRoleWriteAccess> itemsToUpdate = ModuleRoleWriteAccess.Where(s => s.StageStep > ModuleStages_ModuleStep && s.ModuleNameLookup == ModuleStages_ModuleName).ToList();

            if (itemsToDelete.Count > 0)
            {
                RequestRoleWriteAccessManager.Delete(itemsToDelete);
            }

            if (itemsToUpdate.Count > 0)
            {
                //items.StageStep = items.StageStep - 1;
                itemsToUpdate.ForEach(x => { x.StageStep--; });
                RequestRoleWriteAccessManager.UpdateItems(itemsToUpdate);
            }               

            if (deleteLifeCycleStage != null)
                objLifeCycleStageManager.Delete(deleteLifeCycleStage);

        }        
    }
    public interface ILifeCycleStageManager : IManagerBase<LifeCycleStage>
    {

    }
}
