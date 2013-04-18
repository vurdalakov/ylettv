#region License
// YleTtv is a C# class library that simplifies work with YLE teletext (teksti-TV) API
// https://github.com/vurdalakov/ylettv
//
// Copyright (c) 2013 Vurdalakov
// email: vurdalakov@gmail.com

/*
    This file is part of YleTtv class library.

    YleTtv is free software: you can redistribute it and/or modify
    it under the terms of the GNU Lesser General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    YleTtv is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public License
    along with YleTtv.  If not, see <http://www.gnu.org/licenses/>.
*/
#endregion

namespace Vurdalakov.YleTtv
{
    using System;

    public class YleTtvSubpage
    {
        public Int32 Number { get; set; }
        public DateTime Timestamp { get; set; }
        public String[] TextLines { get; set; }
    }
}
