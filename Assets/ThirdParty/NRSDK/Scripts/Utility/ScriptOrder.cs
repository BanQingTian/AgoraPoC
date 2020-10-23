/****************************************************************************
* Copyright 2019 Nreal Techonology Limited. All rights reserved.
*                                                                                                                                                          
* This file is part of NRSDK.                                                                                                          
*                                                                                                                                                           
* https://www.nreal.ai/        
* 
*****************************************************************************/

namespace NRKernal
{
    using System;

    /// <summary>
    /// Ammend the order of script.
    /// </summary>
    public class ScriptOrder : Attribute
    {
        public int order;

        public ScriptOrder(int order)
        {
            this.order = order;
        }
    }
}
