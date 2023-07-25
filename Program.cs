// Decompiled with JetBrains decompiler
// Type: KFM_INCLUDE.WebClientEx
// Assembly: KFM_INCLUDED, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3EC1F72F-985F-4D38-A59E-99A47EC3416E
// Assembly location: C:\Users\s.zhaparov\Desktop\KFM_LOADER_new\KFM_INCLUDE\KFM_INCLUDED.exe

using System;
using System.Net;

namespace KFM_INCLUDE
{
  public class WebClientEx : WebClient
  {
    public CookieContainer CookieContainer { get; private set; }

    public WebClientEx() => this.CookieContainer = new CookieContainer();

    protected override WebRequest GetWebRequest(Uri address)
    {
      WebRequest webRequest = base.GetWebRequest(address);
      if (webRequest is HttpWebRequest)
        (webRequest as HttpWebRequest).CookieContainer = this.CookieContainer;
      return webRequest;
    }
  }
}
