using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ITAnalyticsBL;
using ITAnalyticsBL.DB;
using System.Xml;
using ITAnalyticsUtility;
using System.Data;

namespace ITAnalyticsBL.BL
{
    public class ModelOutputHelper
    {
        /// <summary>
        /// Save Model Output if modeloutputxml is passed then created modeloutxml from
        /// specified ModelVersionID and ModelInputID
        /// </summary>
        /// <param name="modelVersionID"></param>
        /// <param name="modelInputID"></param>
        /// <param name="modelOutputXml"></param>
        /// <returns></returns>
        public static string SaveModelOutput(ModelDB db, long modelVersionID, long modelInputID, string modelOutputXml = "")
        {
            string returnMessage = "Error";
            //Get modelverion and input on that version
            ModelVersion modelV = db.ModelVersions.Find(modelVersionID);
            ModelInput modelInput = db.ModelInputs.Find(modelInputID);
            db.Dispose();
            if (modelV == null)
            {
                returnMessage = "Analytic not found";
            }

            if (modelInput == null)
            {
                returnMessage = "Input on run is not found";
            }

            //if (modelOutputXml == string.Empty && modelV != null && modelInput != null)
            //{
            //    modelOutputXml = QLookService.Process(modelV.ModelXml, modelInput.InputXml);
            //}

            if (modelV != null && modelInput != null && modelOutputXml != string.Empty)
            {
                using (ModelOutputContext outputContext = new ModelOutputContext(db.GetDBContext()))
                {
                    XmlDocument outputXml = new XmlDocument();
                    outputXml.LoadXml(modelOutputXml);
                    ModelOutput modelOutput = GenerateModelOutput(outputXml, modelVersionID, modelInputID);
                    outputContext.ModelOutputs.Add(modelOutput);
                    //outputContext.Entry<ModelOutput>(modelOutput).State = EntityState.Added;
                    outputContext.SaveChanges();
                }
            }
            return returnMessage;
        }

        /// <summary>
        /// Get modeloutput of specified modelversionid nad modelinputid
        /// </summary>
        /// <param name="modelVersionID"></param>
        /// <param name="modelInputID"></param>
        /// <returns></returns>
        public static ModelOutput GetModelOutput(long modelVersionID, long modelInputID)
        {
            ModelOutput modelOutput = null;
            using (ModelOutputContext outputContext = new ModelOutputContext()) 
            {
                modelOutput = outputContext.ModelOutputs.FirstOrDefault(x => x.ModelVersionID == modelVersionID && x.ModelInputID == modelInputID);
            }
            return modelOutput;
        }

        /// <summary>
        /// check modeloutput exist or not of specified modelversionid nad modelinputid
        /// </summary>
        /// <param name="modelVersionID"></param>
        /// <param name="modelInputID"></param>
        /// <returns></returns>
        public static bool IsModelOutputExist(long modelVersionID, long modelInputID)
        {
            using (ModelOutputContext outputContext = new ModelOutputContext())
            {
                ModelOutput modelOutput = outputContext.ModelOutputs.FirstOrDefault(x => x.ModelVersionID == modelVersionID && x.ModelInputID == modelInputID);
                if (modelOutput != null)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Flush model output of specified input
        /// </summary>
        /// <param name="modelVersionID"></param>
        /// <param name="modelInputID"></param>
        /// <returns></returns>
        public static bool FlushModelOutput(long modelVersionID, long modelInputID)
        {
            using (ModelOutputContext outputContext = new ModelOutputContext())
            {
                ModelOutput modelOutput = outputContext.ModelOutputs.FirstOrDefault(x => x.ModelVersionID == modelVersionID && x.ModelInputID == modelInputID);
                if (modelOutput != null)
                {
                    outputContext.ModelOutputs.Remove(modelOutput);
                    //outputContext.Entry<ModelOutput>(modelOutput).State = EntityState.Deleted;
                    outputContext.SaveChanges();
                    return true;
                }
            }
            return false;
        }


        /// <summary>
        ///  Generate modeloutput from outputxml xmldocument
        /// </summary>
        /// <param name="modelVersionID"></param>
        /// <param name="outputXml">Optional param</param>
        /// <param name="modelInputID">Optional param</param>
        /// <returns></returns>
        public static ModelOutput GenerateModelOutput(XmlDocument outputXml,long modelVersionID = 0, long modelInputID = 0)
        {
            XmlElement outputElement = (XmlElement)outputXml.DocumentElement.SelectSingleNode(".//" + XmlTagLibrary.INTERACTION_DATA); ;

            ModelOutput mOutput = new ModelOutput();
            mOutput.ModelVersionID = modelVersionID;
            mOutput.ModelInputID = modelInputID;
            mOutput.ModelInternalID = outputElement.GetAttribute(XmlTagLibrary.MODEL_NAME);

            mOutput.Weight = Convert.ToDouble(outputElement.GetAttribute(XmlTagLibrary.WEIGHT));
            mOutput.ModelName = outputElement.GetAttribute(XmlTagLibrary.MODEL_NAME);
            mOutput.RowScore = Convert.ToDouble(outputElement.GetAttribute(XmlTagLibrary.RawScore));
            mOutput.WeightedScore = Convert.ToDouble(outputElement.GetAttribute(XmlTagLibrary.WEIGHTED_SCORE));
            mOutput.CummulativeWeight = Convert.ToDouble(outputElement.GetAttribute(XmlTagLibrary.CumulativeWeight));
            mOutput.CummulativeScore = Convert.ToDouble(outputElement.GetAttribute(XmlTagLibrary.CumulativeScore));

            XmlNodeList innerModels = outputElement.SelectNodes(".//" + XmlTagLibrary.INNER_MODEL);
            foreach (XmlNode innerModel in innerModels)
            {
              //  mOutput.ModelOutputID = 1;
                if (mOutput.ModelSectionOutputs == null)
                {
                    mOutput.ModelSectionOutputs = new List<ModelSectionOutput>();
                }
                mOutput.ModelSectionOutputs .Add(GetModuleSectionOutput(modelVersionID, (XmlElement)innerModel, modelInputID));
            }

            if (innerModels.Count <= 0)
            {
                XmlNodeList modelNodes = outputElement.SelectNodes(".//" + XmlTagLibrary.NODE);
                foreach (XmlNode node in modelNodes)
                {
                    if (mOutput.ModelSubSectionOutputs == null)
                    {
                        mOutput.ModelSubSectionOutputs = new List<ModelSubSectionOutput>();
                    }
                    mOutput.ModelSubSectionOutputs.Add(GetModuleSubSectionOutput(modelVersionID, (XmlElement)node, modelInputID));
                }
            }
           
            return mOutput;
        }

       

        private static ModelSectionOutput GetModuleSectionOutput(long modelVersionID, XmlElement modelSectionXml, long modelInputID = 0)
        {
            ModelSectionOutput sectionOutput = new ModelSectionOutput();
            sectionOutput.ModelVersionID = modelVersionID;
            sectionOutput.ModelInputID = modelInputID;
            sectionOutput.SectionInternalID = modelSectionXml.GetAttribute(XmlTagLibrary.MODEL_NAME);

           // sectionOutput.ModelSectionOutputID = 1;
           // sectionOutput.ModelOutputID = 1;
            sectionOutput.Weight = Convert.ToDouble(modelSectionXml.GetAttribute(XmlTagLibrary.WEIGHT));
            sectionOutput.SectionName = modelSectionXml.GetAttribute(XmlTagLibrary.MODEL_NAME);
            sectionOutput.RowScore = Convert.ToDouble(modelSectionXml.GetAttribute(XmlTagLibrary.RawScore));
            sectionOutput.WeightedScore = Convert.ToDouble(modelSectionXml.GetAttribute(XmlTagLibrary.WEIGHTED_SCORE));
            sectionOutput.CummulativeWeight = Convert.ToDouble(modelSectionXml.GetAttribute(XmlTagLibrary.CumulativeWeight));
            sectionOutput.CummulativeScore = Convert.ToDouble(modelSectionXml.GetAttribute(XmlTagLibrary.CumulativeScore));

            XmlNodeList nodes = modelSectionXml.SelectNodes(".//" + XmlTagLibrary.NODE);
            foreach (XmlNode node in nodes)
            {
                if (sectionOutput.ModelSubSectionOutputs == null)
                {
                    sectionOutput.ModelSubSectionOutputs = new List<ModelSubSectionOutput>();
                }
                sectionOutput.ModelSubSectionOutputs.Add(GetModuleSubSectionOutput(modelVersionID, (XmlElement)node, modelInputID));
            }

            return sectionOutput;
        }

        private static ModelSubSectionOutput GetModuleSubSectionOutput(long modelVersionID, XmlElement modelSubSectionXml, long modelInputID = 0)
        {
            ModelSubSectionOutput subSectionOutput = new ModelSubSectionOutput();
            subSectionOutput.ModelVersionID = modelVersionID;
            subSectionOutput.ModelInputID = modelInputID;
            subSectionOutput.SubSectionInternalID = modelSubSectionXml.GetAttribute(XmlTagLibrary.ID);


            subSectionOutput.Weight = Convert.ToDouble(modelSubSectionXml.GetAttribute(XmlTagLibrary.WEIGHT));
            subSectionOutput.SubSectionName = modelSubSectionXml.GetAttribute(XmlTagLibrary.ID);
            subSectionOutput.RowScore = Convert.ToDouble(modelSubSectionXml.GetAttribute(XmlTagLibrary.RawScore));
            subSectionOutput.WeightedScore = Convert.ToDouble(modelSubSectionXml.GetAttribute(XmlTagLibrary.WEIGHTED_SCORE));
            subSectionOutput.CummulativeWeight = Convert.ToDouble(modelSubSectionXml.GetAttribute(XmlTagLibrary.CumulativeWeight));
            subSectionOutput.CummulativeScore = Convert.ToDouble(modelSubSectionXml.GetAttribute(XmlTagLibrary.CumulativeScore));

            XmlNodeList variables = modelSubSectionXml.SelectNodes(".//" + XmlTagLibrary.VARIABLE);
            foreach (XmlNode variable in variables)
            {
                if (subSectionOutput.ModelFeatureOutputs == null)
                {
                    subSectionOutput.ModelFeatureOutputs = new List<ModelFeatureOutput>();
                }
                subSectionOutput.ModelFeatureOutputs.Add(GetModuleFeatureOutput(modelVersionID, (XmlElement)variable, modelInputID));
            }

            return subSectionOutput;
        }

        private static ModelFeatureOutput GetModuleFeatureOutput(long modelVersionID, XmlElement modelFeatureXml, long modelInputID = 0)
        {
            ModelFeatureOutput featureOutput = new ModelFeatureOutput();
            featureOutput.ModelVersionID = modelVersionID;
            featureOutput.ModelInputID = modelInputID;

            featureOutput.Weight = Convert.ToDouble(modelFeatureXml.GetAttribute(XmlTagLibrary.WEIGHT));
            featureOutput.FeatureName = modelFeatureXml.GetAttribute(XmlTagLibrary.NAME);
            featureOutput.RowScore = Convert.ToDouble(modelFeatureXml.GetAttribute(XmlTagLibrary.RawScore));
            featureOutput.WeightedScore = Convert.ToDouble(modelFeatureXml.GetAttribute(XmlTagLibrary.WEIGHTED_SCORE));
            featureOutput.CummulativeWeight = Convert.ToDouble(modelFeatureXml.GetAttribute(XmlTagLibrary.CumulativeWeight));
            featureOutput.CummulativeScore = Convert.ToDouble(modelFeatureXml.GetAttribute(XmlTagLibrary.CumulativeScore));

            return featureOutput;
        }
    }
}
